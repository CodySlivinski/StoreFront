using StoreFront.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;

namespace StoreFront.UI.MVC.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _config;
        
        public ContactController(IConfiguration config)
        {
            _config = config;
        }

        
        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            //Message From Contact Form 
            string message = $"You have receiced a new email from your site's contact form!<br/>" +
                $"Sender: {cvm.Name}<br/>Email: {cvm.Email}<br/>Subject: {cvm.Subject}<br/> Message: {cvm.Message}";

            //create message object
            var mm = new MimeMessage();

            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

            mm.Subject = cvm.Subject;

            mm.Body = new TextPart("HTML") { Text = message };

            mm.Priority = MessagePriority.Urgent;
 
            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            //Send Email
            using (var client = new SmtpClient())
            {
                //Connect to the mail server
                client.Connect(_config.GetValue<string>("Credentials:Email:Client"));

                client.Authenticate
               (
               //Username
               _config.GetValue<string>("Credentials:Email:User"),
               //Password
               _config.GetValue<string>("Credentials:Email:Password")
               );

                try
                {

                    client.Send(mm);
                }
                catch (Exception ex)
                {

                    ViewBag.ErrorMessage = $"There was an error processing your request. Pleas try again later." +
                        $"<br/>Error Message: {ex}";

                    return View(cvm);
                }
            }
 
            return View("EmailConfirmation", cvm);
        }

    }
}
