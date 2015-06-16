using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CssFrameworkDefine
{
  
    [Serializable]
    public class UrlNotFounException : SystemException
    {
        public UrlNotFounException() : base() { }
        public UrlNotFounException(string message) : base(message) { }

        public UrlNotFounException(string message, Exception inner) : base(message, inner) { }

        protected UrlNotFounException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
