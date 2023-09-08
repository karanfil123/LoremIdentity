namespace LoremIdentity.Web.MailServices
{
    public interface IMailService
    {
        Task SendResetPasswordEmail(string resetEmailLink, string toEmail);
    }
}