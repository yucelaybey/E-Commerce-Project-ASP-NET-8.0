using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Results.AppUserResults
{
    public class GetAppUserQueryResult
    {
        public int AppUserId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public bool EmailCheck { get; set; }
        public string Password { get; set; }
        public DateTime BirtDay { get; set; }
        public string PhoneNumber { get; set; }
    }
}
