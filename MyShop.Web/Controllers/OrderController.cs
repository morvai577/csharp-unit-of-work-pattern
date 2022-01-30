using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Web.Models;

namespace MyShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var orders = _unitOfWork.OrderRepository.Find(order => order.OrderDate > DateTime.UtcNow.AddDays(-1)).ToList();

            return View(orders);
        }

        public IActionResult Create()
        {
            var products = _unitOfWork.ProductRepository.All().ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            // Model Validation
            if (!model.LineItems.Any()) return BadRequest("Please submit line items");
            if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");
            
            
            // Check if customer already exist by name and if yes we don't want to create new one
            var customer = _unitOfWork.CustomerRepository.Find(c => c.Name == model.Customer.Name).FirstOrDefault();

            if (customer != null)
            {
                // Customer already exists so just update their details
                customer.ShippingAddress = model.Customer.ShippingAddress;
                customer.PostalCode = default;
                customer.City = model.Customer.City;
                customer.Country = model.Customer.Country;

                _unitOfWork.CustomerRepository.Update(customer);
            }

            // If customer doesn't exist then go inside this condition and create new customer
            else
            {
                customer = new Customer()
                {
                    Name = model.Customer.Name,
                    ShippingAddress = model.Customer.ShippingAddress,
                    City = model.Customer.City,
                    PostalCode = model.Customer.PostalCode,
                    Country = model.Customer.Country
                };
            }

            var order = new Order
            {
                LineItems = model.LineItems
                    .Select(line => new LineItem { ProductId = line.ProductId, Quantity = line.Quantity })
                    .ToList(),

                Customer = customer
            };

            _unitOfWork.OrderRepository.Add(order);

            _unitOfWork.SaveChanges();

            return Ok("Order Created");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
