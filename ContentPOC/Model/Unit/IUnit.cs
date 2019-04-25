using ContentPOC.Unit;
using ContentPOC.Unit.Model;

namespace ContentPOC.Model
{
    public interface IUnit
    {
        Meta Meta { get; }
        
        string Namespace { get; }
    }
}