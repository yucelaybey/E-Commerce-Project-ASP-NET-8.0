using E_Commerce.Persistence.Migrations;
using ECommerce.WebUI.SendEmailService;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;
using System.Drawing;
using System.Runtime.InteropServices;
using NuGet.Protocol.Plugins;
using System.Security.Policy;

namespace ECommerce.WebUI.SendEmailService
{
    public class EmailService
    {
        public void SendPasswordResetEmail(string email, string name,int code)
        {
            string codeWithSpaces = ConvertCodeToDigitsWithSpaces(code);
            // Email mesajını oluştur
            var message = CreateEmailMessage(email, name, codeWithSpaces);

            // Email'i gönder
            SendEmail(message);
        }

        public string ConvertCodeToDigitsWithSpaces(int code)
        {
            string numberAsString = code.ToString();
            return string.Join(" ", numberAsString.ToCharArray());
        }

        private MimeMessage CreateEmailMessage(string email, string name, string code)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Amazon Şifre Sıfırlama", "hastanebilgiyonetimsistemi@gmail.com"));
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = "Şifre Sıfırlama İsteği";
            string cssContent = System.IO.File.ReadAllText("wwwroot/css/mail.css");
            // HTML içeriği oluştur
            string htmlContent = GenerateHtmlContent(code, cssContent);

            // HTML içeriğini email body olarak ayarla
            message.Body = new TextPart("html")
            {
                Text = htmlContent
            };

            return message;
        }

        private string GenerateHtmlContent(string code,string css)
        {
         
            string htmlContent = $@"
<!DOCTYPE html>
<html lang=""tr"">

<head>
    <style>
        {css}
    </style>
</head>

<body>
    <div class=""mail-container"">
        <!-- Mail Header -->
        <div class=""mail-header"">
            <h1>Şifre Sıfırlama Kodu</h1>
        </div>

        <!-- Mail Content -->
        <div class=""mail-content"">
            <p>Merhaba,</p>
            <p>Şifrenizi sıfırlamak için aşağıdaki kodu kullanabilirsiniz:</p>

            <!-- Kod Container -->
            <div class=""code-container"">
                <div class=""code"">{code}</div> 
            </div>

            <p>Bu kod doğrulama sayfası kapanana geçerlidir. Eğer bu talebi siz yapmadıysanız, lütfen bu maili dikkate almayın.</p>
            <p>Teşekkürler,</p>
            <p><strong>Amazon</strong></p>
        </div>

        <!-- Mail Footer -->
        <div class=""mail-footer"">
            <p>Bu bir otomatik maildir, lütfen cevaplamayınız.</p>
            <p>Eğer sorun yaşıyorsanız, <a href=""#"">destek ekibimizle iletişime geçin</a>.</p>
        </div>
    </div>
</body>

</html>";

            return htmlContent;
        }

        private void SendEmail(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("hastanebilgiyonetimsistemi@gmail.com", "fqay ktlh togf jghx");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}