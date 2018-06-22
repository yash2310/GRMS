using System;
using System.ComponentModel;
using System.Net.Mail;

namespace AMS.ApplicationCore.Utilities
{
    public class SendMail
    {
        public static bool Send(string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("amssupport@bluepi.in");
                mail.To.Add(email);
                mail.Subject = "You reset your password";
                mail.Body = "Hi,\n\n        Your password has been reset successfully ! \n\nThanks ";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("amssupport@bluepi.in", "bluepi.1234");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void SendAsyncMail(string email)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("manoj.kumar@bluepi.in");
            mail.To.Add(new MailAddress(email));
            mail.Subject = "You reset your password for AMS";
            mail.Body = "Hi,\n\n        Your password has been reset successfully ! \n\nThanks ";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            Object state = mail;

            //event handler for asynchronous call
            smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
            try
            {
                smtpClient.SendAsync(mail, state);
            }
            catch (Exception ex) { /* exception handling code here */ }
        }

        static void smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MailMessage mail = e.UserState as MailMessage;

            if (!e.Cancelled && e.Error != null)
            {
                //message.text = "mail sent successfully";
            }
        }
        //static bool mailSent = false;
        //private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        //{
        //    // Get the unique identifier for this asynchronous operation.
        //    String token = (string)e.UserState;

        //    if (e.Cancelled)
        //    {
        //        Console.WriteLine("[{0}] Send canceled.", token);
        //    }
        //    if (e.Error != null)
        //    {
        //        Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
        //    }
        //    else
        //    {
        //        Console.WriteLine("Message sent.");
        //    }
        //    mailSent = true;
        //}
        //public void SendNow(string[] args)
        //{
        //    // Command line argument must the the SMTP host.
        //    SmtpClient client = new SmtpClient(args[0]);
        //    // Specify the e-mail sender.
        //    // Create a mailing address that includes a UTF8 character
        //    // in the display name.
        //    MailAddress from = new MailAddress("manoj.kumar@gmail.com","Manoj Kumar",System.Text.Encoding.UTF8);
        //    // Set destinations for the e-mail message.
        //    MailAddress to = new MailAddress("ben@contoso.com");
        //    // Specify the message content.
        //    MailMessage message = new MailMessage(from, to);
        //    message.Body = "This is a test e-mail message sent by an application. ";
        //    // Include some non-ASCII characters in body and subject.
        //    string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
        //    message.Body += Environment.NewLine + someArrows;
        //    message.BodyEncoding = System.Text.Encoding.UTF8;
        //    message.Subject = "test message 1" + someArrows;
        //    message.SubjectEncoding = System.Text.Encoding.UTF8;
        //    // Set the method that is called back when the send operation ends.
        //    client.SendCompleted += new
        //    SendCompletedEventHandler(SendCompletedCallback);
        //    // The userState can be any object that allows your callback 
        //    // method to identify this send operation.
        //    // For this example, the userToken is a string constant.
        //    string userState = "test message1";
        //    client.SendAsync(message, userState);
        //    Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
        //    string answer = Console.ReadLine();
        //    // If the user canceled the send, and mail hasn't been sent yet,
        //    // then cancel the pending operation.
        //    if (answer.StartsWith("c") && mailSent == false)
        //    {
        //        client.SendAsyncCancel();
        //    }
        //    // Clean up.
        //    message.Dispose();
        //}
    }
}
