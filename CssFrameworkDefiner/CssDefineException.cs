using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CssFrameworkDefine
{
    
        [Serializable]
        public class CssDefineException : SystemException
        {
            public CssDefineException() : base() { }
            public CssDefineException(string message) : base(message) { }

            public CssDefineException(string message, Exception inner) : base(message, inner) { }

            protected CssDefineException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        }
}
