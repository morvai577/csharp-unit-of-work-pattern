using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyShop.Infrastructure;

namespace MyShop.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly  IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(Guid? id)
        {
            if (id == null)
            {
                var customers = _unitOfWork.CustomerRepository.All().ToList();

                return View(customers);
            }
            else
            {
                var customer = _unitOfWork.CustomerRepository.Get(id.Value);

                return View(new[] { customer });
            }
        }
    }
}
