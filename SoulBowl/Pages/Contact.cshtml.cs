using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;

namespace SoulBowl.Pages
{
    public class ContactModel : PageModel
    {
        public string customername { get; set; }
        public string customeremail { get; set; }
        public string customermessage { get; set; }
        const string appPassword = "ssjqseckwavrytni";
        public string myStatus;
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            myStatus = "Here";
            // Prevents invalid inputs from being entered
            // The forms are automatically set up to prevent invalid inputs (e.g. by inserting letters in a phone number)
            if (!ModelState.IsValid)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("CustomerContactFieldError", "Invalid data for customer contact form");
                myStatus = "ModelError";
                return Page();
            }

            try
            {
                myStatus = "Trying to send";
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new NetworkCredential("soulbowl.rest@gmail.com", appPassword);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("soulbowl.rest@gmail.com", "Soul Bowl Website Enquiry");
                mailMessage.To.Add(new MailAddress("2126677@chester.ac.uk"));
                //mailMessage.CC.Add(new MailAddress(@customeremail));

                myStatus = "Start Message compose subject";
                mailMessage.Body = "Hello, \n" +
                        "A customer has contacted Soul Bowl :\n" +
                        "Message: " + customermessage + "\n";
                mailMessage.Subject = "Soul Bowl - New Enquiry From Website";
                smtpClient.Send(mailMessage);
                
            }
            catch (Exception ex)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("CustomerContactFieldError", "Invalid data for customer contact form");
                // End of adapted code
                return Page();
            }
            return RedirectToPage("/ContactConfirm");
            //return Page();

        }
    }
}


