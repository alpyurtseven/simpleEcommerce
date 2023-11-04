using Microsoft.AspNetCore.Mvc;
using EcommerceProject.Models;

namespace EcommerceProject.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products
            .OrderByDescending(p => p.ProductId)
            .Take(8)
            .ToList();

        return View(products);
    }
}

