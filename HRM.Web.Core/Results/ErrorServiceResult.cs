using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Web.Core
{
    public class ErrorServiceResult
    {
        public string DevMessage { get; set; }
        public string UserMessage { get; set; }
        public int ErrorCode { get; set; }
        public string TraceId { get; set; }
        public string MoreInfo { get; set; }
    }
}
