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
        [HttpGet, Route("api/customer/purchases/{CustomerId}")]
        // returns all orders for a given customer
        public IEnumerable<Order> GetByOrder(int CustomerId) => _dataContext.Orders.Where(o => o.CustomerId == CustomerId).OrderBy(o => o.OrderId);
        [HttpGet, Route("api/customer/purchases/purchasedetails/{OrderId}")]
        // // returns specfic order, ordered by product name
        public IEnumerable<OrderDetail> GetByOrderDetail(int OrderId) => _dataContext.OrderDetails.Where(o => o.OrderId == OrderId).OrderBy(o => o.ProductId);
        // returns all reviews for a specfic product based on a specfic customer
        public IEnumerable<Review> GetProductReview(int ProductId, int CustomerId) => _dataContext.Reviews.Where(r => r.ProductId == ProductId && r.CustomerId == CustomerId).OrderBy(r => r.Product.ProductName);
        [HttpPost, Route("api/addtocart")]
        // adds a row to the cartitem table
        public CartItem Post([FromBody] CartItemJSON cartItem) => _dataContext.AddToCart(cartItem);
    }
}