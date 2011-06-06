using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPlatform.Core
{
    /// <summary>
    /// The supported operations in where-extension
    /// </summary>
    public enum WhereOperation
    {
        [StringValue("eq")]
        Equal,
        [StringValue("ne")]
        NotEqual,
        [StringValue("cn")]
        Contains,
        [StringValue("gt")]
        Greater,
        [StringValue("ge")]
        GreaterOrEqual,
        [StringValue("lt")]
        Less,
        [StringValue("le")]
        LessOrEqual
    }
}