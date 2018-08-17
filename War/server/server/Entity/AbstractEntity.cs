using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    public abstract class AbstractEntity
    {
        protected AbstractEntity()
        {

        }

        public virtual int? PKValue { get; set; }
    }
}
