using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
  // this controller depends on the NorthwindRepository
  private DataContext _dataContext;
  public ProductController(DataContext db) => _dataContext = db;
  public IActionResult Category() => View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  public IActionResult Index(int id){
    ViewBag.id = id;
    return View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  }
  public IActionResult Reviews(int ProductId) => View(_dataContext.Products.FirstOrDefault(p => p.ProductId == ProductId).Reviews.ToList());
  // public IActionResult Reviews() => View(
  //       (from r in _dataContext.Reviews
  //       join p in _dataContext.Products on r.ProductId equals p.ProductId
  //       join od in _dataContext.OrderDetails on p.ProductId equals od.ProductId
  //       join o in _dataContext.Orders on od.OrderId equals o.OrderId
  //       where o.Customer.Email == User.Identity.Name
  //       select r).OrderBy(r => r.ProductId).ToList()
  //   );
  // public IActionResult Detail(int id){
  //   ViewBag.id = id;
  //   return View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  //   }
}
