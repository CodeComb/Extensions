﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Mvc
{
    public abstract class BaseController<TContext> : BaseController
    {
        [Inject]
        public virtual TContext DB { get; set; }
    }
}
