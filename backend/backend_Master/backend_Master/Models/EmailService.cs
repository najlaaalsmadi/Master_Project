using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using System.Threading.Tasks;

public class EmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Najlaa AL-Smadi", "najlaak399@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", toEmail)); // احذف الاسم هنا، لأنه ليس ضروريًا.
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain")
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("najlaak399@gmail.com", "apxy vexd stma iomf"); // تأكد من صحة كلمة المرور

                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                // معالجة الخطأ (يمكنك تسجيل الخطأ أو إعادة رميه)
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
