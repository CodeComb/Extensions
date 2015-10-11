using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using CodeComb.AspNet.Extensions.Sample.Models;

namespace CodeComb.AspNet.Extensions.Sample.Controllers
{
    public class BaseController : BaseController<SampleContext, Models.User, string>
    {

    }
}
