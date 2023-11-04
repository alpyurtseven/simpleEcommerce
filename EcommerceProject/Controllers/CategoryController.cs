using EcommerceProject.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult List(int id)
        {
            List<Product> products = _context.Products.ToList();

            if (id != 0)
            {
                products = products.Where(z => z.CategoryId == id).ToList();
            }

            return View(products);
        }
    }
}

