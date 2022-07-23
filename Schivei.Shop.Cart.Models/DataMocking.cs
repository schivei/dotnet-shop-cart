using System.Collections.Concurrent;

namespace Schivei.Shop.Cart.Models;

/// <summary>
/// Used to mock database
/// </summary>
public static class DataMocking
{
    static DataMocking()
    {
        Products = new Product[] {
            new()
            {
                Brand = "Apple",
                Id = Guid.NewGuid(),
                Model = "X",
                Name = "IPhone X",
                Variations = new()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Color = new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Black"
                        },
                        Size = new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "32g"
                        },
                        Sku = "CELAPLIPXBL32",
                        Price = 99.90m,
                        SellingPrice = 69.90m,
                        Stock = 10,
                        ShortDescription = "Apple Iphone X Black 32GB",
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Color = new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Black"
                        },
                        Size = new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "63g"
                        },
                        Sku = "CELAPLIPXBL64",
                        Price = 129.90m,
                        SellingPrice = 99.90m,
                        Stock = 10,
                        ShortDescription = "Apple Iphone X Black 64GB",
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Color = new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Black"
                        },
                        Size = new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "128g"
                        },
                        Sku = "CELAPLIPH0XSGXR",
                        Price = 169.90m,
                        SellingPrice = 159.90m,
                        Stock = 10,
                        ShortDescription = "Apple Iphone X Black 128GB",
                    },
                }
            }
        };

        var prd = Products[0];
        foreach (var variation in prd.Variations)
            variation.Product = prd;

        Carts = new();
    }

    public static Product[] Products { get; }

    public static ConcurrentBag<Cart> Carts { get; }
}
