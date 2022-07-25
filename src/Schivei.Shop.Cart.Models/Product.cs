namespace Schivei.Shop.Cart.Models;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string Thumbnail { get; set; } = string.Empty;

    public HashSet<ProductVariation> Variations { get; set; } = new();
}

public class ProductColor
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class ProductSize
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class ProductVariation
{
    public Guid Id { get; set; }

    public Product Product { get; set; } = default!;

    public ProductColor? Color { get; set; }

    public ProductSize? Size { get; set; }

    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal SellingPrice { get; set; }
    public long Stock { get; set; }
    public string ShortDescription { get; set; } = string.Empty;
}
