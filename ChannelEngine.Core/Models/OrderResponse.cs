using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChannelEngine.Core.Models
{

    public class OrderResponse
    {
        public List<Order> Content { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int ItemsPerPage { get; set; }
        public int StatusCode { get; set; }
        public int? LogId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public ValidationErrors ValidationErrors { get; set; }
    }
}
