using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repositories.DTO
{
    public abstract class BaseCustomerDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }
    }
}
