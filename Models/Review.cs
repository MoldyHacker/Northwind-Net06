using System.ComponentModel.DataAnnotations;
public class Review
{
    public int RatingId {get; set;}
    [Required(ErrorMessage ="Review is required")]
    [Range(0,5, ErrorMessage = "Must be 0-5")]
    public int Rating {get; set;}
    public string Comment {get; set;}
    public int CustomerId { get; set; }
    public int ProductId {get; set;}
    public Product Product {get; set;}
}