using System.Data;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Details(int id)
        {
            Product product = _context.Products
                   .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();

            ViewBag.Categories = new SelectList(categories,
                "CategoryId",
                "Name");

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Details",
                    new { id = product.ProductId });
            }
            catch
            {
                return View(product);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Product product = _context.Products
                .FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Categories.ToList();

            ViewBag.Categories = new SelectList(categories,
                "CategoryId",
                "Name",
                product.CategoryId);

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            try
            {
                _context.Products.Update(product);
                _context.SaveChanges();

                return RedirectToAction("Details",
                    new { id = product.ProductId });
            }
            catch
            {
                return View(product);
            }
        }
    }
}

