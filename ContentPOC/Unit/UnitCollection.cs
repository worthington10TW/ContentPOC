using ContentPOC.Unit;
using System.Collections.Generic;

namespace ContentPOC
{
    public abstract class UnitCollection : List<IUnit>, IUnit
    {
        public abstract string UnitType { get; }

        public Meta Meta => new Meta(this);
    }
}