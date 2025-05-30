namespace ECommerce.WebUI.SendEmailService
{
    public static class RandomGenerateNumber
    {
        public static int GenerateSixDigitCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999); // 100000 ile 999999 arasında rastgele bir sayı üretir
        }
    }
}
