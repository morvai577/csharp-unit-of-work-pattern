using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Models;

namespace MyShop.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>
{
    public OrderRepository(ShoppingContext context) : base(context)
    {
    }

    /// <summary>
    /// Override find method to fetch line items and products data linked to the order via only one query
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
    {
        return context.Orders
            .Include(o => o.LineItems)
            .ThenInclude(l => l.Product)
            .Where(predicate)
            .ToList();
    }

    /// <summary>
    /// Override update functionality of generic repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override Order Update(Order entity)
    {
        var order = context.Orders
            .Include(o => o.LineItems)
            .ThenInclude(l => l.Product)
            .Single();

        order.OrderDate = entity.OrderDate;
        order.LineItems = entity.LineItems;
        
        return base.Update(entity);
    }
}