using ByteartRetail.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteartRetail.Application
{
    public static class QuerySpecExtension
    {
        public static bool IsVerbose(this QuerySpec spec)
        {
            return spec.Verbose ?? false;
        }
    }
}
