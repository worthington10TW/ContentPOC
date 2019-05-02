using ContentPOC.Unit.Model;
using System.Collections.Generic;

namespace ContentPOC.Model
{
    public interface IUnit
    {
        IMeta Meta { get; }
        
        string[] Domain { get; }

        List<IUnit> Children { get; }
    }
}