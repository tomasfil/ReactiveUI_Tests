using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RxTests
{
    public class MessageBusWaysToAvoidUsingMessageBusTest
    {
        public class DocumentViewModel : ReactiveObject
        {
            public ReactiveCommand<Unit, Unit> Close { get; set; }

            public DocumentViewModel()
            {
                // Note that we don't actually *subscribe* to Close here or implement
                // anything in DocumentViewModel, because Closing is a responsibility
                // of the document list.
                Close = ReactiveCommand.Create(() => { });
            }
        }

        public class MainViewModel : ReactiveObject
        {
            public ObservableCollection<DocumentViewModel> OpenDocuments { get; protected set; }

            public MainViewModel()
            {
                OpenDocuments = new ObservableCollection<DocumentViewModel>();

                // Whenever the list of documents change, calculate a new Observable
                // to represent whenever any of the *current* documents have been
                // requested to close, then Switch to that. When we get something
                // to close, remove it from the list.
                OpenDocuments
                    .ToObservableChangeSet()
                    .AutoRefreshOnObservable(document => document.Close)
                    .Select(s => WhenAnyDocumentClosed())
                    .Switch()
                    .Do(_=>Debug.WriteLine("Will be subscribed now"))
                    .Subscribe(x => OpenDocuments.Remove(x));
            }

            IObservable<DocumentViewModel> WhenAnyDocumentClosed()
            {
                // Select the documents into a list of Observables
                // who return the Document to close when signaled,
                // then flatten them all together.
                var list = OpenDocuments
                    .Select(x => x.Close.Select(_ => x)).ToList();

                return OpenDocuments
                    .Select(x => x.Close.Select(_ => x))
                    .Merge();
            }
        }

        [Fact]
        public void TestWithSingleDocument()
        {
            var main = new MainViewModel();
            var doc = new DocumentViewModel();
            main.OpenDocuments.Add(doc);

            doc.Close.Execute().Subscribe();
            var actual = main.OpenDocuments.Count;

            Assert.Equal(0, actual);
        }

        [Fact]
        public void TestWithFirstFromMultipleDocuments()
        {
            var main = new MainViewModel();
            var doc = new DocumentViewModel();
            main.OpenDocuments.Add(doc);
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(new DocumentViewModel());

            doc.Close.Execute().Subscribe();
            var actual = main.OpenDocuments.Count;

            Assert.Equal(4, actual);
        }

        [Fact]
        public void TestWithCenterFromMultipleDocuments()
        {
            var main = new MainViewModel();
            var doc = new DocumentViewModel();
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(doc);
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(new DocumentViewModel());

            doc.Close.Execute().Subscribe();
            var actual = main.OpenDocuments.Count;

            Assert.Equal(4, actual);
        }

        [Fact]
        public void TestWithLastFromMultipleDocuments()
        {
            var main = new MainViewModel();
            var doc = new DocumentViewModel();
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(new DocumentViewModel());
            main.OpenDocuments.Add(doc);

            doc.Close.Execute().Subscribe();
            var actual = main.OpenDocuments.Count;

            Assert.Equal(4, actual);
        }
    }
}
