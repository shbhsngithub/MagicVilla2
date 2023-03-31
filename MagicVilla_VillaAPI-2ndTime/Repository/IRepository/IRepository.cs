using System.Linq.Expressions;

namespace MagicVilla_VillaAPI_2ndTime.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,string? includeProperties=null );
        //Task<IEnumerable<Villa>> GetAll();
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
        //Task<Villa> Get(int id);
        Task CreateAsync(T entity);
      
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
