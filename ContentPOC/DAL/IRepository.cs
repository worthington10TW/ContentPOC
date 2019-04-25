using ContentPOC.Unit;
using System.Threading.Tasks;

namespace ContentPOC.DAL
{
    public interface IRepository
    {
        Task<IUnit> SaveAsync(IUnit unit);

        IUnit Get(Id id);
    }
}
