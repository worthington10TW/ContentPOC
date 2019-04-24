using Newtonsoft.Json;
using System.Collections.Generic;

namespace ContentPOC
{
    public abstract class UnitCollection : Unit.Unit
    {
        List<IUnit> Units { get; }
    }
}