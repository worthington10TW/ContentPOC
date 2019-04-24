using System;
using System.Collections.Generic;

namespace ContentPOC.Unit
{
    public class News : List<IUnit>, IUnit
    {
        public string Href => "news/1234567";
        
        public string Headline { get; set; }

        public string Summary { get; set; }

        public string Story { get; set; }
    }
}
