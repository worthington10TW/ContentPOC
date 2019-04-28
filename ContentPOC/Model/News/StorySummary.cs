﻿namespace ContentPOC.Unit.Model.News
{
    public class StorySummary : Unit
    {
        public StorySummary(string value) => Value = value;

        public override string Namespace => "news/story-summary";

        public string Value { get; }

        public override string ToString() => Value.ToString();
    }
}