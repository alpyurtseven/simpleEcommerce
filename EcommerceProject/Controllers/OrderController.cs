using EcommerceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcommerceProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult AddToOrder(int productId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {

                var order = JsonConvert
                    .DeserializeObject<Order>(
                    HttpContext.Session
                    .GetString("order") ?? "") ?? new Order();

                var orderItem = new OrderItem
                {
                    Product = product,
                    ProductId = product.ProductId,
                    Quantity = 1,
                    OrderItemId = order.OrderItems.Count() + 1,
                    
                };

                order.OrderItems.Add(orderItem);

                HttpContext.Session
                    .SetString("order", JsonConvert.SerializeObject(order));
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {

            var order = JsonConvert
                        .DeserializeObject<Order>(
                        HttpContext.Session
                        .GetString("order") ?? "") ?? new Order();

            return View(order);
        }

        public IActionResult OrderSuccess()
        {
            HttpContext.Session
             .SetString("order", "");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            try
            {
                foreach (var item in order.OrderItems)
                {
                    item.Product = null;
                }

                order.OrderDate = DateTime.Now;

                _context.Orders.Add(order);
                _context.SaveChanges();

                return RedirectToAction("OrderSuccess");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult RemoveFromOrder(int orderItemId)
        {
            var order = JsonConvert
                              .DeserializeObject<Order>(
                               HttpContext.Session
                              .GetString("order") ?? "") ?? new Order();

            var orderItem = order.OrderItems
                .FirstOrDefault(oi => oi.OrderItemId == orderItemId);

            if (orderItem != null)
            {
                order.OrderItems.Remove(orderItem);

                 HttpContext.Session
                 .SetString("order", JsonConvert.SerializeObject(order));
            }

            return RedirectToAction("Index");
        }

    }
}
