using System.Net.Mail;
using System.Text;

namespace CalendarMate.Services;

public interface IEmailSender
{
    bool SendMail(string address, string subject, string body);
}

public class EmailSender : IEmailSender
{
    public bool SendMail(string address, string subject, string body)
    {
        // 생각보다 복잡한데 나중에 따로 메일서버 만들어서 구현해야할듯?
        // 임시로 네이버 메일 사용
        try
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("zenpia@naver.com");
            message.To.Add(address);

            message.Subject = subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.naver.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential("zenpia", "chltkdgms108*");
            smtp.Send(message);

            return true;
        }
        catch
        {
            return false;
        }
    }
}