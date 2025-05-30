using NuGet.Protocol.Core.Types;

namespace ECommerce.WebUI.Models
{
    public class JwtResponseModel
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
