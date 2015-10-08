using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Http;
using Microsoft.Dnx.Runtime;

namespace CodeComb.AspNet.Extensions.Template
{
    public class Template
    {
        public TemplateCollection Collection { get; }

        public TemplateInfo Current
        {
            get
            {
                var ret = Collection.Templates.Where(x => x.Identifier == Provider.DetermineRequestTemplate()).FirstOrDefault();
                if (ret ==null)
                    ret = Collection.Templates.Where(x => x.IsDefault).FirstOrDefault();
                if (ret == null)
                    throw new FileNotFoundException();
                return ret;
            }
        }

        public TemplateInfo Default
        {
            get
            {
                var ret = Collection.Templates.Where(x => x.IsDefault).FirstOrDefault();
                if (ret == null)
                    ret = Collection.Templates.FirstOrDefault();
                if (ret == null)
                    throw new FileNotFoundException();
                return ret;
            }
        }

        public IRequestTemplateProvider Provider { get; }

        public Template(IRequestTemplateProvider templateProvider, TemplateCollection collection)
        {
            Collection = collection;
            Provider = templateProvider;
        }
    }
}
