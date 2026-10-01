namespace LabWork4.Odd.Filtering;

public class ProductFilter : QueryParameters
{
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? InStockOnly { get; set; }
}
