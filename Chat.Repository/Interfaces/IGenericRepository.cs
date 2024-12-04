using Chat.Repository.Specifications;

namespace Chat.Repository.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<IReadOnlyList<TEntity>> GetAllAsNoTrackingAsync(ISpecifications<TEntity> specs);
        public Task<TEntity> GetByIdAsync(Guid? id);
    }
}
