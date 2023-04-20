using System.ComponentModel.DataAnnotations;
public class Rating
{
    public int RatingId {get; set;}
    [Required(ErrorMessage ="Review is required")]
    [Range(0,5, ErrorMessage = "Must be 0-5")]
    public int Review {get; set;}
    public string Comment {get; set;}
    public int ProductId {get; set;}
    public Product Product {get; set;}


}