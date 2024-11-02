using ElectronicShop.Common.Model;
using System.Net.Mail;
using System.Text;

namespace ElectronicShop.Common.Helper
{
    public class MailHelper
    {
        public MailHelper(EmailConfig config)
        {
            this.Config = config;
        }

        private EmailConfig Config { get; set; } = new();

        public async Task<string> Send(string mailTo, string subject, string content)
        {
            try
            {
                if (Config.IsActive)
                {
                    StringBuilder Body = new();
                    Body.Append(content);

                    MailMessage mail = new(new MailAddress(Config.UserName, Config.DisplayName), new MailAddress(mailTo))
                    {
                    };

                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.Body = Body.ToString();
                    mail.To.Add(mailTo);

                    SmtpClient smtp = new()
                    {
                        Host = Config.Host,
                        Port = Config.Port,
                        Credentials = new System.Net.NetworkCredential(Config.UserName, Config.Password),
                        EnableSsl = true,
                        UseDefaultCredentials = true
                    };

                    Task.Run(() => smtp.SendMailAsync(mail));
                }

                return mailTo;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> Send(string mailTo, string subject, string content, IEnumerable<string> CCs)
        {
            try
            {
                if (Config.IsActive)
                {
                    StringBuilder Body = new();
                    Body.Append(content);

                    MailMessage mail = new(new MailAddress(Config.UserName, Config.DisplayName), new MailAddress(mailTo))
                    {
                    };

                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.Body = Body.ToString();

                    foreach (var mailCC in CCs)
                    {
                        mail.CC.Add(mailCC);
                    }

                    SmtpClient smtp = new()
                    {
                        Host = Config.Host,
                        Port = Config.Port,
                        Credentials = new System.Net.NetworkCredential(Config.UserName, Config.Password),
                        EnableSsl = true,
                        //UseDefaultCredentials = true
                    };

                    Task.Run(() => smtp.SendMailAsync(mail));
                }

                return mailTo;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}