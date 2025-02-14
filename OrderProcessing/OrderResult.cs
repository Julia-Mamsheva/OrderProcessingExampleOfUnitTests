﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing
{
    public class OrderResult
    {
        public bool Success { get; }
        public string Message { get; }

        public OrderResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
