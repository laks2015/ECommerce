using Ecommerce.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repositories.DTO
{
    public partial class OrderDto
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string? OrderNumber { get; set; }

        public int CustomerId { get; set; }

        public decimal? TotalAmount { get; set; }
               
    }
}
