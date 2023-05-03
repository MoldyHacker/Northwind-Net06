using System.ComponentModel.DataAnnotations;
public class ProductReview
{
    public int ProductReviewId { get; set; }
    public string ProductName { get; set; }
    public int Rating { get; set; }
}