using MagicVilla_VillaAPI_2ndTime.Model;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI_2ndTime.Repository.IRepository
{
    public interface IVillaNumberRepository:IRepository<VillaNumber>
    {
      
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
      

    }
}
