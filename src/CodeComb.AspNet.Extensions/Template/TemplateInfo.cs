using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CodeComb.AspNet.Extensions.Template
{
    public class TemplateInfo
    {
        [JsonIgnore]
        public string Identifier { get; set; }

        public bool IsDefault { get; set; }

        public string Author { get; set; }

        public string License { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }
    }
}
