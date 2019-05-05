using System;
using System.Security.Cryptography;
using System.Text;
using ContentPOC.Model;
using ContentPOC.Unit.Model;
using Newtonsoft.Json;

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
    }
}
