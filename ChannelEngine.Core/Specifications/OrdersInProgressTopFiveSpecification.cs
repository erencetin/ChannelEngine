using ChannelEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChannelEngine.Core.Specifications
{
    public class OrdersInProgressTopFiveSpecification : BaseSpecification<Order>
    {
        public OrdersInProgressTopFiveSpecification() : base(x => x.Status == "IN PROGRESS")
        {
            AddOrderByDescending(o => o.CreatedAt); //Degistirilecek
            ApplyTaking(5);
        }
    }
}
