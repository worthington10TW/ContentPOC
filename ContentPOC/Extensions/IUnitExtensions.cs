using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ContentPOC.Model;
using ContentPOC.Unit.Model;
using ContentPOC.Unit.Model.News;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContentPOC.Extensions
{
    public static class IUnitExtensions
    {
        public static Guid ToGuid<TUnit>(this TUnit unit) where TUnit : IUnit
        {
            using (var md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(
                      string.Format("{0:X}",
                          JsonConvert.SerializeObject(
                              unit,
                              new JsonSerializerSettings

                              { ContractResolver = new IgnoreMetaSerializeContractResolver() }))));
                return new Guid(hash);
            }
        }

        public static IUnit ToUnit(this string[] area, Guid id, Dictionary<string, object> data)
        {
            if (area == null) throw new ArgumentNullException(nameof(area));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            IUnit unit = null;
            if (area.SequenceEqual(new Headline("").Domain))
                unit = new Headline(data["Value"].ToString());

            if (area.SequenceEqual(new StoryText("").Domain))
                unit = new StoryText(data["Value"].ToString());

            if (area.SequenceEqual(new StorySummary("").Domain))
                unit = new StorySummary(data["Value"].ToString());

            if (area.SequenceEqual(new NewsItem().Domain)) 
                unit = new NewsItem(
                    ((JArray)data["Children"]).Select(x => new Guid(x.Value<string>()))
                    .ToArray());

            unit?.Meta?.SetId(id);
            return unit ?? throw new NotImplementedException();
        }
    }
}
