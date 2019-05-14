using System;
using System.Collections.Generic;
using System.Text;

namespace ContentPOC.Integration.Endpoints
{
    public class Dto
    {
        public class AllNewsDto 
        {
            public Meta meta { get; set; }
            public string value {get;set;}
        }
        public class NewsDto
        {
            public Meta meta { get; set; }
            public Child[] children { get; set; }
        }
        
        public class Meta
        {
            public string href { get; set; }
        }

        public class Child
        {
            public string value { get; set; }
            public Meta meta { get; set; }
        }
    }
}
