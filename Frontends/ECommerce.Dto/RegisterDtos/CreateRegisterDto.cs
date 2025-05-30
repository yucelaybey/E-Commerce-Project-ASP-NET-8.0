using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.RegisterDto
{
    public class CreateRegisterDto
    {
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public bool EmailConfirm { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public string PhoneNumber { get; set; }
    }
}
