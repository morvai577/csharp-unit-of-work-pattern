using System.Linq;
using MyShop.Domain.Models;

namespace MyShop.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>
{
    public ProductRepository(ShoppingContext context) : base(context)
    {
    }

    /// <summary>
    /// Override update functionality of generic repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override Product Update(Product entity)
    {
        var product = context.Products.Single(p => p.ProductId == entity.ProductId);

        product.Name = entity.Name;
        product.Price = entity.Price;

        return base.Update(product);
    }
}