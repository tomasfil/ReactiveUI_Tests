using DynamicData;
using DynamicData.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicData_SortAndLargeSetOfDataIssue
{
    public static class ReactiveUiExtensions
    {
        public static IObservable<IChangeSet<TDestination, TKey>> TransformWithInlineUpdate<TObject, TKey, TDestination>(this IObservable<IChangeSet<TObject, TKey>> source,
       Func<TObject, TDestination> transformFactory,
       Action<TDestination, TObject> updateAction = null)
        {
            return source.Scan((ChangeAwareCache<TDestination, TKey>)null, (cache, changes) =>
            {
                if (cache == null)
                    cache = new ChangeAwareCache<TDestination, TKey>(changes.Count);

                foreach (var change in changes)
                {
                    switch (change.Reason)
                    {
                        case ChangeReason.Add:
                            cache.AddOrUpdate(transformFactory(change.Current), change.Key);
                            break;
                        case ChangeReason.Update:
                            {
                                if (updateAction == null) continue;

                                var previous = cache.Lookup(change.Key)
                                    .ValueOrThrow(() => new MissingKeyException($"{change.Key} is not found."));

                                updateAction(previous, change.Current);

                                //send a refresh as this will force downstream operators 
                                cache.Refresh(change.Key);
                            }
                            break;
                        case ChangeReason.Remove:
                            cache.Remove(change.Key);
                            break;
                        case ChangeReason.Refresh:
                            cache.Refresh(change.Key);
                            break;
                        case ChangeReason.Moved:
                            //Do nothing !
                            break;
                    }
                }
                return cache;
            }).Select(cache => cache.CaptureChanges());
        }
    }
}
