using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class CustomerController : Controller
{
    // this controller depends on the DataContext & the UserManager
    private DataContext _dataContext;
    private UserManager<AppUser> _userManager;
    public CustomerController(DataContext db, UserManager<AppUser> usrMgr)
    {
        _dataContext = db;
        _userManager = usrMgr;
    }
    public IActionResult Register() => View();
    [HttpPost, ValidateAntiForgeryToken]
    public async System.Threading.Tasks.Task<IActionResult> Register(CustomerWithPassword customerWithPassword)
    {
        if (ModelState.IsValid)
        {
            Customer customer = customerWithPassword.Customer;
            if (_dataContext.Customers.Any(c => c.CompanyName == customer.CompanyName))
            {
                ModelState.AddModelError("", "Company Name must be unique");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    AppUser user = new AppUser
                    {
                        // email and username are synced - this is by choice
                        Email = customer.Email,
                        UserName = customer.Email
                    };
                    // Add user to Identity DB
                    IdentityResult result = await _userManager.CreateAsync(user, customerWithPassword.Password);
                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result);
                    }
                    else
                    {
                        // Assign user to customers Role
                        result = await _userManager.AddToRoleAsync(user, "northwind-customer");

                        if (!result.Succeeded)
                        {
                            // Delete User from Identity DB
                            await _userManager.DeleteAsync(user);
                            AddErrorsFromResult(result);
                        }
                        else
                        {
                            // Create customer (Northwind)
                            _dataContext.AddCustomer(customer);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
        }
        return View();
    }
    [Authorize(Roles = "northwind-customer")]
    public IActionResult Account() => View(_dataContext.Customers.FirstOrDefault(c => c.Email == User.Identity.Name));
    [Authorize(Roles = "northwind-customer"), HttpPost, ValidateAntiForgeryToken]
    public IActionResult Account(Customer customer)
    {
        // Edit customer info
        _dataContext.EditCustomer(customer);
        return RedirectToAction("Index", "Home");
    }
    private void AddErrorsFromResult(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    }
    [Authorize(Roles = "northwind-customer")]
    // public IActionResult Purchases() => View(_dataContext.Orders.OrderBy(o => o.OrderId).Where(o => o.Customer.Email == User.Identity.Name));
    public IActionResult Purchases() => View(
        (from p in _dataContext.Products
        join od in _dataContext.OrderDetails on p.ProductId equals od.ProductId
        join o in _dataContext.Orders on od.OrderId equals o.OrderId
        where o.Customer.Email == User.Identity.Name
        select p).Distinct().OrderBy(p => p.ProductName).ToList()
    );
    // TODO: Create custom model to update data for the page. 

    // [Authorize(Roles = "northwind-customer"), HttpPost, ValidateAntiForgeryToken]
    // public IActionResult Purchases(Customer customer)
    // {
    //     // Edit customer info
    //     _dataContext.EditCustomer(customer);
    //     return RedirectToAction("Index", "Home");
    // }
    public IActionResult PurchaseDetail(int id){
    // ViewBag.id = id;
    return View(_dataContext.OrderDetails.OrderBy(o => o.OrderDetailId).Where(o => o.OrderId == id));
  }
    public IActionResult Review()=>View();
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Review(Review review)
    {
        if(ModelState.IsValid)
        {
            _dataContext.AddReview(review);
            
        }
        return RedirectToAction("PurchaseDetail");

    }

    [Authorize(Roles = "northwind-customer")]
    public IActionResult Reviews() => View(_dataContext.Reviews.Where(r => r.Customer.Email == User.Identity.Name).OrderBy(r => r.DateTime));

}