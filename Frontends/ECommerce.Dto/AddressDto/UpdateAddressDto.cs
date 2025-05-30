using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.AddressDto
{
    public class UpdateAddressDto
    {
        public int addressID { get; set; }
        public string addressTitle { get; set; }
        public string nameSurname { get; set; }
        public string phoneNumber { get; set; }
        public string city { get; set; }
        public string town { get; set; }
        public string quarter { get; set; }
        public string district { get; set; }
        public string addressDetails { get; set; }
        public bool isDefault { get; set; }
    }
}
