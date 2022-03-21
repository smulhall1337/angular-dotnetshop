using System.Collections;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

// https://www.c-sharpcorner.com/UploadFile/b1df45/unit-of-work-in-repository-pattern/

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreContext _context;
    private Hashtable _repositories;

    public UnitOfWork(StoreContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        // instead of instantiating repositories and each one having their own context, we can simply 
        // use this to instantiate any repository we need just once and use one context
        if (_repositories == null)
        {
            _repositories = new Hashtable();
        }

        var type = typeof(TEntity).Name;
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<TEntity>) _repositories[type];
    }

    public async Task<int> Complete()
    {
        // save all the tracked changes.
        // if an error occurs, none of the changes are saved and everything is rolled back
        return await _context.SaveChangesAsync();
    }
}