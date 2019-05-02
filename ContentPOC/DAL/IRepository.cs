using ContentPOC.Model;
using ContentPOC.Unit.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContentPOC.DAL
{
    public interface IRepository
    {
        Task<IUnit> SaveAsync(IUnit unit);

        IUnit Get(string[] domain, Id id);

        List<IUnit> GetAll(string[] domain);
    }
}
