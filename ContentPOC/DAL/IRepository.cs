using ContentPOC.Model;
using ContentPOC.Unit;
using ContentPOC.Unit.Model;
using System.Threading.Tasks;

namespace ContentPOC.DAL
{
    public interface IRepository
    {
        Task<IUnit> SaveAsync(IUnit unit);

        IUnit Get(string area, Id id);
    }
}
