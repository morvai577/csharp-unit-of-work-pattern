using MyShop.Domain.Models;
using MyShop.Infrastructure.Repositories;

namespace MyShop.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ShoppingContext _context;

    public UnitOfWork(ShoppingContext context)
    {
        _context = context;
    }

    private IRepository<Customer> _customerRepository;
    public IRepository<Customer> CustomerRepository => _customerRepository ??= new CustomerRepository(_context);

    // This field stores the same instance to the repository once it has been initialised first time
    private IRepository<Order> _orderRepository;
    public IRepository<Order> OrderRepository =>
        /*
         * The following is an example of lazy initialisation where we store the initialisation of the order repository in a private field
         */
        _orderRepository ??= new OrderRepository(_context);

    private IRepository<Product> _productRepository;
    public IRepository<Product> ProductRepository => _productRepository ??= new ProductRepository(_context);

    public void SaveChanges()
    {
        _context.SaveChanges(); // Applies all transactions at once
    }
}