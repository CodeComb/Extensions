using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Dnx.Runtime;
using Newtonsoft.Json;

namespace CodeComb.AspNet.Extensions.Template
{
    public class TemplateCollection
    {
        public List<TemplateInfo> Templates { get; private set; }

        public int Count { get { return Templates.Count; } }

        private IApplicationEnvironment _env { get; set; }

        public TemplateInfo Find(string identifier)
        {
            var ret = Templates.Where(x => x.Identifier == identifier).FirstOrDefault();
            if (ret == null)
                Templates.Where(x => x.IsDefault).FirstOrDefault();
            if (ret == null)
                Templates.Where(x => x.IsDefault).First();
            if (ret == null)
                ret = null;
            return ret;
        }

        public TemplateCollection(IApplicationEnvironment env)
        {
            _env = env;
            Refresh();
        }

        public void Refresh()
        {
            var tmp = new List<TemplateInfo>();
            var path = _env.ApplicationBasePath + "/Views/";
            if (Directory.Exists(path))
            {
                var directories = Directory.GetDirectories(path);
                foreach (var d in directories)
                {
                    System.Diagnostics.Debug.WriteLine(d + "/template.json");
                    try
                    {
                        if (File.Exists(d + "/template.json"))
                        {
                            var jsonStr = File.ReadAllText(d + "/template.json");
                            var template = JsonConvert.DeserializeObject<TemplateInfo>(jsonStr);
                            var lst1 = d.LastIndexOf('/');
                            var lst2 = d.LastIndexOf('\\');
                            var lst = Math.Max(lst1, lst2);
                            template.Identifier = d.Substring(lst + 1);
                            tmp.Add(template);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            path = _env.ApplicationBasePath + "/Templates/";
            if (Directory.Exists(path))
            {
                var directories = Directory.GetDirectories(path);
                foreach (var d in directories)
                {
                    System.Diagnostics.Debug.WriteLine(d + "/template.json");
                    try
                    {
                        if (File.Exists(d + "/template.json"))
                        {
                            var jsonStr = File.ReadAllText(d + "/template.json");
                            var template = JsonConvert.DeserializeObject<TemplateInfo>(jsonStr);
                            var lst1 = d.LastIndexOf('/');
                            var lst2 = d.LastIndexOf('\\');
                            var lst = Math.Max(lst1, lst2);
                            template.Identifier = d.Substring(lst + 1);
                            tmp.Add(template);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            Templates = tmp;
            GC.Collect();
        }
    }
}
