using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.AppUserDto
{
    public class CheckEmailResponseDto
    {
        public int Id { get; set; }
        public bool EmailExists { get; set; }
        public string Email { get; set; }
        public string NameSurname { get; set; }
    }
}
