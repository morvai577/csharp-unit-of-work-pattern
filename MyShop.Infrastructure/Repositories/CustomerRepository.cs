using System.Linq;
using MyShop.Domain.Models;

namespace MyShop.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>
{
    public CustomerRepository(ShoppingContext context) : base(context)
    {
    }

    /// <summary>
    /// Override update functionality of generic repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override Customer Update(Customer entity)
    {
        var customer = context.Customers.Single(c => c.CustomerId == entity.CustomerId);

        customer.Name = entity.Name;
        customer.City = entity.City;
        customer.PostalCode = entity.PostalCode;
        customer.ShippingAddress = entity.ShippingAddress;
        customer.Country = entity.Country;

        return base.Update(customer);
    }
}