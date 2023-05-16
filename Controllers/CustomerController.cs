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


        // public IActionResult Purchases() => View(_dataContext.Orders.OrderBy(o => o.OrderId).Where(o => o.Customer.Email == User.Identity.Name));
    [Authorize(Roles = "northwind-customer")]
    public IActionResult Purchases() => View(

        (from p in _dataContext.Products
        join od in _dataContext.OrderDetails on p.ProductId equals od.ProductId
        join o in _dataContext.Orders on od.OrderId equals o.OrderId
        where o.Customer.Email == User.Identity.Name
        where !_dataContext.Reviews.Any(r => r.CustomerId == o.CustomerId && r.ProductId == p.ProductId)
        select p).Distinct().OrderBy(p => p.ProductName).ToList()
    );


    public IActionResult InputReview(int id){
        ViewBag.id = id;
        return View();
    }
    
    [Authorize(Roles = "northwind-customer"), HttpPost, ValidateAntiForgeryToken]
    public IActionResult InputReview(Review review)
    {  
    if (review.Rating == 0)
    {
        ModelState.AddModelError("review.Rating", "Rating is required.");
    }

    Customer customer = _dataContext.Customers.FirstOrDefault(c => c.Email == User.Identity.Name);
    
    if (customer == null)
    {
        ModelState.AddModelError("", "Customer not found.");
    }

    if (!ModelState.IsValid)
    {
        return View(review); // Return the view with model errors displayed
    }
else {
    try
    {
        review.CustomerId = customer.CustomerId;
        _dataContext.InputReview(review);
        return RedirectToAction("Purchases");
    }
    catch (Exception ex)
    {
        // Log the exception or handle it as needed
        ModelState.AddModelError("", "An error occurred while saving the review.");
        return View(review); // Return the view with the generic error message
    }
}
    

    
    }
}