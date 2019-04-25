using ContentPOC.Unit;
using ContentPOC.Unit.Model;
using System.Collections.Generic;

namespace ContentPOC.Model
{
    public abstract class UnitCollection : List<IUnit>, IUnit
    {
        public abstract string UnitType { get; }

        public Meta Meta => new Meta(this);
    }
}