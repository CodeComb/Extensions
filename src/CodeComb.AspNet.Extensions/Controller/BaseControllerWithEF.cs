using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Mvc
{
    public abstract class BaseController<TContext> : BaseController
    {
        [FromServices]
        public virtual TContext DB { get; set; }

        public override void Prepare()
        {
            base.Prepare();
        }
    }
}
