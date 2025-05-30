using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.LoginDtos
{
    public class LoginResponseDto
    {
        public int AppUserId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
    }
}
