using MagicVilla_VillaAPI_2ndTime.Model;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI_2ndTime.Repository.IRepository
{
    public interface IVillaRepository:IRepository<Villa>
    {
      
        Task<Villa> UpdateAsync(Villa entity);
      

    }
}
