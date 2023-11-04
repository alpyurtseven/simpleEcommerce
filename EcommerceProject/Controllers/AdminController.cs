using System.Data;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<Admin> _userManager; 
        private readonly SignInManager<Admin> _signInManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<Admin> userManager, SignInManager<Admin> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(model.UserName,
                    model.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Geçersiz giriş denemesi.");
                    return View(model);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Products()
        {
            List<Product> products = _context.Products.ToList();

            return View(products);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Orders()
        {
            List<Order> orders = _context.Orders
                .Include(z=>z.OrderItems)
                .ThenInclude(z=>z.Product)
                .ToList();

            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult OrderDetail(int id)
        {
            Order order = _context.Orders
                .Include(z => z.OrderItems)
                .ThenInclude(z => z.Product)
                .Where(z=>z.OrderId == id)
                .FirstOrDefault();

            return View(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Approve(Order order)
        {
            try
            {
                var orderEntity = _context.Orders
               .Where(z => z.OrderId == order.OrderId)
               .FirstOrDefault();

                orderEntity.Status = true;

                _context.Orders.Update(orderEntity);
                _context.SaveChanges();

                return RedirectToAction("Orders");
            }
            catch
            {
                return RedirectToAction("OrderDetail", order);
            }
        }
    }
}

