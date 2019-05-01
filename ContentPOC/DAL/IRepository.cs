using ContentPOC.Model;
using ContentPOC.Unit.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContentPOC.DAL
{
    public interface IRepository
    {
        Task<IUnit> SaveAsync(IUnit unit);

        IUnit Get(string area, Id id);

        List<IUnit> GetAll(string area);
    }
}
