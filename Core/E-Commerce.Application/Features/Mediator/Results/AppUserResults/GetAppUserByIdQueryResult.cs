using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Results.AppUserResults
{
    public class GetAppUserByIdQueryResult
    {
        public int AppUserId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
    }
}
