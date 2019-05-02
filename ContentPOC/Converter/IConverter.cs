using ContentPOC.Model;
using System.Threading.Tasks;
using System.Xml;

namespace ContentPOC.Converter
{
    public interface IConverter<T> where T : IUnit
    {
        Task<IUnit> CreateAsync(XmlDocument xml);
    }
}