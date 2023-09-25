using Cara.Core.Interfaces;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<T> _table;
        public Repository(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }
        public async Task<T?> GetAsync(int? id)
        {
            return await _table.FindAsync(id);
        }
        public async Task CreateAsync(T entity)
        {
            await _table.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _table.Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
