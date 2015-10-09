using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CodeComb.AspNet.Extensions.Sample.Models
{
    public class SampleContext : IdentityDbContext<User>
    {
    }
}
