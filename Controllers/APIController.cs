using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Controllers
{
    public class APIController : Controller
    {
        // this controller depends on the NorthwindRepository
        private DataContext _dataContext;
        public APIController(DataContext db) => _dataContext = db;

        [HttpGet, Route("api/product")]
        // returns all products
        public IEnumerable<Product> Get() => _dataContext.Products.OrderBy(p => p.ProductName);
        
        [HttpGet, Route("api/product/{id}")]
        // returns specific product
        public Product Get(int id) => _dataContext.Products.FirstOrDefault(p => p.ProductId == id);
        
        [HttpGet, Route("api/product/discontinued/{discontinued}")]
        // returns all products where discontinued = true/false
        public IEnumerable<Product> GetDiscontinued(bool discontinued) => _dataContext.Products.Where(p => p.Discontinued == discontinued).OrderBy(p => p.ProductName);
        
        [HttpGet, Route("api/category")]
        // returns all categories
        public IEnumerable<Category> GetCategory() => _dataContext.Categories.Include("Products").OrderBy(c => c.CategoryName);
        
        [HttpGet, Route("api/category/{CategoryId}/product")]
        // returns all products in a specific category
        public IEnumerable<Product> GetByCategory(int CategoryId) => _dataContext.Products.Where(p => p.CategoryId == CategoryId).OrderBy(p => p.ProductName);
        
        [HttpGet, Route("api/category/{CategoryId}/product/discontinued/{discontinued}")]
        // returns all products in a specific category where discontinued = true/false
        public IEnumerable<Product> GetByCategoryDiscontinued(int CategoryId, bool discontinued) => _dataContext.Products.Where(p => p.CategoryId == CategoryId && p.Discontinued == discontinued).OrderBy(p => p.ProductName);
        [HttpGet, Route("api/product/{productId}/reviews")]
        public IEnumerable<Review> GetProductReviews(int productId) => _dataContext.Reviews.Include("Customer").Include("Product").Where(p => p.ProductId == productId).OrderBy(d => d.DateTime);
        [HttpGet, Route("api/category/{CategoryId}/productwithreviews")]
        public IEnumerable<ProductReview> GetByCategoryWithRating(int CategoryId){
            var products = _dataContext.Products.Include("Reviews").Where(p => p.CategoryId == CategoryId).OrderBy(p => p.ProductName);
            List<ProductReview> productReviews = new List<ProductReview>();
                foreach(var p in products){
                    ProductReview pr = new ProductReview();
                    pr.ProductId = p.ProductId;
                    pr.ProductName = p.ProductName;
                    pr.UnitPrice = p.UnitPrice;
                    pr.UnitsInStock = p.UnitsInStock;
                    pr.Discontinued = p.Discontinued;
                    int r_total = 0;
                    foreach(Review r in p.Reviews){
                        r_total += r.Rating;
                    }
                    pr.AvgRating = (decimal)r_total / p.Reviews.Count();
                    productReviews.Add(pr);
                };
            return productReviews;
        }
        [HttpGet, Route("api/category/{CategoryId}/productwithreviews/discontinued/{discontinued}")]
        // returns all products in a specific category where discontinued = true/false
        public IEnumerable<ProductReview> GetByCategoryDiscontinuedWithRating(int CategoryId, bool discontinued){
            var products = _dataContext.Products.Include("Reviews").Where(p => p.CategoryId == CategoryId && p.Discontinued == discontinued).OrderBy(p => p.ProductName);
            List<ProductReview> productReviews = new List<ProductReview>();
            foreach(var p in products){
                ProductReview pr = new ProductReview();
                pr.ProductId = p.ProductId;
                pr.ProductName = p.ProductName;
                pr.UnitPrice = p.UnitPrice;
                pr.UnitsInStock = p.UnitsInStock;
                int r_total = 0;
                foreach(Review r in p.Reviews){
                    r_total += r.Rating;
                }
                pr.AvgRating = (decimal)r_total / p.Reviews.Count();
                productReviews.Add(pr);
            };
            return productReviews;
        }
        [HttpGet, Route("api/review")]
        public IEnumerable<Review> GetAllReviews()=>_dataContext.Reviews.OrderBy(r => r.ReviewId);
        
        // [HttpGet, Route("api/review/{ProductId}")]
        // // public IEnumerable<Review> GetProductReviews(int ProductId) => _dataContext.Reviews.Include("Customer").Where(r => r.ProductId == ProductId);
        // // public IEnumerable<Review> GetProductReview(int ProductId, int CustomerId) => _dataContext.Reviews.Where(r => r.ProductId == ProductId && r.CustomerId == CustomerId).OrderBy(r => r.Product.ProductName);
        
        [HttpPost, Route("api/addtocart")]
        // adds a row to the cartitem table
        public CartItem Post([FromBody] CartItemJSON cartItem) => _dataContext.AddToCart(cartItem);

        // [HttpPost, Route("api/addreview")]
        // // adds a row to the cartitem table
        // public CartItem Post([FromBody] CartItemJSON cartItem) => _dataContext.AddToCart(cartItem);
    }
}