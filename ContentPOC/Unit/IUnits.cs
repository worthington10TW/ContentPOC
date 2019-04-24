using System.Collections.Generic;

namespace ContentPOC
{
    public interface IUnits : IUnit
    {
        List<IUnit> Units { get; }
    }
}