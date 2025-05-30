namespace ECommerce.WebUI.Models
{
    public class PasswordResetModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string NameSurname { get; set; }
        public string Code { get; set; }
        public string EnteredCode { get; set; } // Kullanıcının girdiği kod
    }
}
