namespace GraphQL.AutoUnions.Tests;

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

public class ProductSubscription
{
    public static IObservable<ProductUnion> NewProducts()
    {
        var observable = Observable.Create<ProductUnion>(async (observer, cancellationToken) =>
        {
            observer.OnNext(ProductData.Books.First());
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            observer.OnNext(ProductData.Electronics.First());
        });

        return observable;
    }
}
