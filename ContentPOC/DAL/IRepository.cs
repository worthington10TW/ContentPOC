using ContentPOC.Model;
using ContentPOC.Unit.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContentPOC.DAL
{
    public interface IRepository
    {
        Task<IUnit> SaveAsync(IUnit unit);

        IUnit Get(string[] domain, Guid id);

        List<IUnit> GetAll(string[] domain);
    }
}
