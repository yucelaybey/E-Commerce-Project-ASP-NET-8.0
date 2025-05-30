using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class AppUser
    {
        public int AppUserId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public bool EmailCheck { get; set; }
        public string Password { get; set; }
        public int AppRoleId { get; set; }
        public AppRole AppRole { get; set; }
        public DateTime BirtDay { get; set; }
        public string PhoneNumber { get; set; }
    }
    
}
