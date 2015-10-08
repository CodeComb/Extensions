using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeComb.AspNet.Extensions.Template
{
    public interface IRequestTemplateProvider
    {
        string DetermineRequestTemplate();
    }
}
