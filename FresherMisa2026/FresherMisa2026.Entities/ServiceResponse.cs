using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Entities
{
    public class ServiceResponse
    {
        public bool IsSuccess { get; set; }
        
        public int Code { get; set; }

        public object Data { get; set; }

        public object UserMessage { get; set; }

        public object DevMessage { get; set; }
    }
}
