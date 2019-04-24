using System.Threading.Tasks;

namespace ContentPOC.Converter
{
    public interface IConverter<T> where T : IUnit
    {
        Task<IUnit> CreateAsync(NewsRequestXml xml);
    }
}