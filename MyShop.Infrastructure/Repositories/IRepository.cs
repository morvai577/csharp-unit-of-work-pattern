using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyShop.Infrastructure.Repositories;

/// <summary>
/// This interface is used to dictate a contract with the repository.
/// This interface is in charge of the CRUD operations we want to perform with the data.
/// Notice the use of generic...<T>... this is to keep things generic for max extensibility.
/// </summary>
public interface IRepository<T>
{
    T Add(T entity);
    T Update(T entity);
    T Get(Guid id);
    IEnumerable<T> All();
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate); // Takes an expression and returns a bool
    void SaveChanges();
}