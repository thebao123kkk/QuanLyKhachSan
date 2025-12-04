using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;

public static class EmailService
{
    private static readonly string smtpServer = "smtp.gmail.com";
    private static readonly int smtpPort = 587;

    private static readonly string senderEmail = "db.hotel.6868@gmail.com";
    private static readonly string senderPassword = "lbqstlkjscbjttmu";

    // 1) GỬI EMAIL BÌNH THƯỜNG (KHÔNG ĐÍNH KÈM)
    public static async Task<bool> SendEmailAsync(
        string toEmail,
        string subject,
        string bodyHtml)
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail, "Hotel Management");
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = bodyHtml;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi gửi email: " + ex.Message);
            return false;
        }
    }

    // 2) GỬI EMAIL CÓ ĐÍNH KÈM FILE (PDF HÓA ĐƠN)
    public static async Task<bool> SendEmailWithAttachmentAsync(
        string toEmail,
        string subject,
        string bodyHtml,
        string attachmentPath)
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail, "Hotel Management");
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = bodyHtml;
            mail.IsBodyHtml = true;

            // File đính kèm (PDF)
            if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
            {
                mail.Attachments.Add(new Attachment(attachmentPath));
            }
            else
            {
                MessageBox.Show("Không tìm thấy file PDF để đính kèm: " + attachmentPath);
            }

            SmtpClient smtp = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi gửi email có file đính kèm: " + ex.Message);
            return false;
        }
    }
}
