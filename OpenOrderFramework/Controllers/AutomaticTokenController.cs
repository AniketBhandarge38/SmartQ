using OpenOrderFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace OpenOrderFramework.Controllers
{
    public class AutomaticTokenController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();

        // GET: AutomaticToken
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AutoTokengeneration()
        {
            int id = storeDB.Orders.Select(i => i.OrderId).DefaultIfEmpty(1).Max();
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            var order = new Order();

            Token token = new Token()
            {
                TokenID = storeDB.Tokens.Select(t => t.TokenID).DefaultIfEmpty(0).Max() + 1,
                OrderId = id,
                TokenDT = storeDB.Tokens.Select(d => d.TokenDT).DefaultIfEmpty(DateTime.Now).Max().AddMinutes(15)

            };

            storeDB.Tokens.Add(token);
            storeDB.SaveChanges();

            //var td = storeDB.Tokens.Select(t => t.TokenDT).DefaultIfEmpty(DateTime.Now).Max();
            // CheckoutController.SendOrderMessage(order.FirstName, "New Order: " + order.OrderId,order.ToString(order), appConfig.OrderEmail);
            //SendOrderMessage(order.FirstName, "Please pick your order. : " + order.OrderId, order.ToString(order), order.Email, td);


            if (isValid)
            {
                return View();
            }
            else
            {
                return View("Error");
            }
        }


        public ActionResult AutoTokenStatus()
        {
            var md = new List<Token>();
            var token = new Token();
            int id = storeDB.Orders.Select(i => i.OrderId).DefaultIfEmpty(1).Max();
            token.TokenID = storeDB.Tokens.Select(t => t.TokenID).DefaultIfEmpty(0).Max();
            token.TokenDT = storeDB.Tokens.Select(t => t.TokenDT).DefaultIfEmpty(DateTime.Now).Max();
            token.OrderId = id;
            md.Add(token);
            return View(md);
        }

        private static void SendOrderMessage(String toName, String subject, String body, String destination, DateTime td)
        {
            
            var fromAddress = new MailAddress("aniket071298@gmail.com", "Aniket Bhandarge");
            var toAddress = new MailAddress(destination, "Customer");
            const string fromPassword = "aniketahen";


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body + td

            })
            {
                smtp.Send(message);
            }


        }


    }
}