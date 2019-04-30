using System.Diagnostics;

namespace ContentPOC.Model.News
{
    [DebuggerDisplay("Location = {Location}")]
    public class RawNewsContentIngested : IEvent
    {
        public string Location { get; set; }
    }
}