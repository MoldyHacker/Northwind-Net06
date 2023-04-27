using System.ComponentModel.DataAnnotations;
public class Review
{
    public int ReviewId {get; set;}
    [Required(ErrorMessage = "Review is required")]
    [Range(1,5, ErrorMessage = "Must be 1-5")]
    public int Rating {get; set;}
    public string Comment {get; set;}
    public DateTime DateTime { get; set; }
    [Required]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    [Required]
    public int ProductId {get; set;}
    public Product Product {get; set;}
}