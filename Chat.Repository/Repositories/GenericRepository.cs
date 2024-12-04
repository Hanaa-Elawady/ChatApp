using Chat.Data.Contexts;
using Chat.Repository.Interfaces;
using Chat.Repository.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Chat.Repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ChatDbContext _context;

        public GenericRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        => await _context.Set<TEntity>().AddAsync(entity);  

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsNoTrackingAsync(ISpecifications<TEntity> specs)
             => await SpecificationsEvaluator<TEntity>.GetQuery(_context.Set<TEntity>(), specs).ToListAsync();


        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

        public Task<TEntity> GetByIdAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
