using System.ComponentModel.DataAnnotations;
public class ProductReview
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
    public decimal AvgRating { get; set; }
}