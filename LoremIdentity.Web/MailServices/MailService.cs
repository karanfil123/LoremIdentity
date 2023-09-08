using LoremIdentity.Web.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace LoremIdentity.Web.MailServices
{
    public class MailService : IMailService
    {
        private readonly EmailSettings _emailSettings;

        public MailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetEmailLink, string toEmail)
        {
            var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Host = _emailSettings.Host;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(toEmail);

            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Local | Şifre Sıfırlama Linki";
            mailMessage.Body = @$"<h4>Şifreniiz yenilemek için aşağıdaki linke tıklayınız<h4/>  <p><a href='{resetEmailLink}'>Şifre Sıfırla<a/><p/>";
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}