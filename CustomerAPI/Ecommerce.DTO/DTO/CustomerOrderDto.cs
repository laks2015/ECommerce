using Ecommerce.DAL.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repositories.DTO
{
    public class CustomerOrderDto : OrderDto
    {
        public virtual List<OrderItemDto>? OrderItems { get; set; }

    }
}
