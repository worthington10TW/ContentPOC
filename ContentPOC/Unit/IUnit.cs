using ContentPOC.Unit;

namespace ContentPOC
{
    public interface IUnit
    {
        Meta Meta { get; }
        
        string UnitType { get; }
    }
}