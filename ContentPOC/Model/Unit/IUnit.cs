using ContentPOC.Unit.Model;
using System.Collections.Generic;

namespace ContentPOC.Model
{
    public interface IUnit
    {
        Meta Meta { get; }
        
        string Namespace { get; }

        List<IUnit> Children { get; }
    }
}