namespace ContentPOC.Model.News
{
    public class RawNewsContentIngested : IEvent
    {
        public string Location { get; set; }
    }
}