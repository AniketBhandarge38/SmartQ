using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenOrderFramework.Configuration;
using OpenOrderFramework.Models;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OpenOrderFramework.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        AppConfigurations appConfig = new AppConfigurations();

        public List<String> CreditCardTypes { get { return appConfig.CreditCardType;} }
        
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            ViewBag.CreditCardTypes = CreditCardTypes;
            var previousOrder = storeDB.Orders.FirstOrDefault(x => x.Username == User.Identity.Name);

            if (previousOrder != null)
                return View(previousOrder);
            else
                return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(FormCollection values)
        {
            ViewBag.CreditCardTypes = CreditCardTypes;
            
            string result =  values[9];
            
            var order = new Order();
            TryUpdateModel(order);
            order.CreditCard = result;

            try
            {
                    order.Username = User.Identity.Name;
                    order.Email = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    var currentUserId = User.Identity.GetUserId();

                    if (order.SaveInfo && !order.Username.Equals("guest@guest.com"))
                    {
                        
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                        var ctx = store.Context;
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        currentUser.Address = order.Address;
                        currentUser.City = order.City;
                        currentUser.Country = order.Country;
                        currentUser.State = order.State;
                        currentUser.Phone = order.Phone;
                        currentUser.PostalCode = order.PostalCode;
                        currentUser.FirstName = order.FirstName;

                        //Save this back
                        //http://stackoverflow.com/questions/20444022/updating-user-data-asp-net-identity
                        //var result = await UserManager.UpdateAsync(currentUser);
                        await ctx.SaveChangesAsync();

                        await storeDB.SaveChangesAsync();
                    }
                    

                    //Save Order
                    storeDB.Orders.Add(order);
                    await storeDB.SaveChangesAsync();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    order = cart.CreateOrder(order);

               // var token = new Token();

               // int id = storeDB.Orders.Select(i => i.OrderId).DefaultIfEmpty(1).Max();
               // bool isValid = storeDB.Orders.Any(
               //o => o.OrderId == id &&
               //o.Username == User.Identity.Name);

                

               // token.TokenID = storeDB.Tokens.Select(t => t.TokenID).DefaultIfEmpty(0).Max() + 1;
               // token.OrderId = id;
               


               // token.TokenDT = DateTime.Now;
               // if (ModelState.IsValid)
               // {
               //     storeDB.Tokens.Add(token);
               //     await storeDB.SaveChangesAsync();

               // }

                //Remove Below Comment to send mail 
          //     SendOrderMessage(order.FirstName, "Please pick your order. : " + order.OrderId,order.ToString(order),order.Email,token.TokenSlot.ToString());
                return RedirectToAction("TokenStatus",
                        new { id = order.OrderId });
                           }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }



        public ActionResult SelectTokenType(int id)
        {
            bool isValid = storeDB.Orders.Any(
               o => o.OrderId == id &&
               o.Username == User.Identity.Name);

            return View(id);
        }


        ////
        //// GET: /Checkout/Complete
        //public async Task<ActionResult> Complete([Bind(Include = "TokenSlot")] Token token)
        //{
        //    int id = storeDB.Orders.Select(i => i.OrderId).DefaultIfEmpty(1).Max();
           
        //    bool isValid = storeDB.Orders.Any(
        //        o => o.OrderId == id &&
        //        o.Username == User.Identity.Name);


        //    token.TokenID = storeDB.Tokens.Select(t => t.TokenID).DefaultIfEmpty(0).Max() + 1;
        //    token.OrderId = id;
        //   // token.TokenSlot = storeDB.Tokens.Select(t => t.TokenSlot).DefaultIfEmpty(" ").LastOrDefault();
        //    token.TokenDT = storeDB.Tokens.Select(d => d.TokenDT).DefaultIfEmpty(DateTime.Now).Max().AddMinutes(15);
        //    if (ModelState.IsValid)
        //    {
        //        storeDB.Tokens.Add(token);
        //       await storeDB.SaveChangesAsync();
                
        //    }

            


        //        var order = new Order();
        //    order.Username = User.Identity.Name;
        //    order.Email = User.Identity.Name;
        //    order.OrderDate = DateTime.Now;
        //    var currentUserId = User.Identity.GetUserId();
        //    if (order.SaveInfo && !order.Username.Equals("guest@guest.com"))
        //    {

        //        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //        var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
        //        var ctx = store.Context;
        //        var currentUser = manager.FindById(User.Identity.GetUserId());

        //        currentUser.Address = order.Address;
        //        currentUser.City = order.City;
        //        currentUser.Country = order.Country;
        //        currentUser.State = order.State;
        //        currentUser.Phone = order.Phone;
        //        currentUser.PostalCode = order.PostalCode;
        //        currentUser.FirstName = order.FirstName;
        //    }

        //    var cart = ShoppingCart.GetCart(this.HttpContext);
        //    order = cart.CreateOrder(order);

        //    var td = storeDB.Tokens.Select(t => t.TokenSlot).DefaultIfEmpty();
        //    SendOrderMessage(order.FirstName, "Please pick your order. : " + order.OrderId, order.ToString(order), order.Email, td.ToString());


        //    if (isValid)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return View("Error");
        //    }

        //}


        public ActionResult TokenStatus(Token token)
        {
            var md = new List<Token>();
            
            token.TokenID = storeDB.Tokens.Select(t => t.TokenID).DefaultIfEmpty(0).Max();
            token.TokenDT = DateTime.Now;
            token.TokenSlot = storeDB.Orders.Select(t => t.Slot).Max();
            token.OrderId= storeDB.Orders.Select(i => i.OrderId).DefaultIfEmpty(1).Max();
            storeDB.Tokens.Add(token);
            storeDB.SaveChangesAsync();


            // var order = new Order();

            //  SendOrderMessage(order.FirstName, "Please pick your order. : " + order.OrderId, order.ToString(order), order.Email, tok.TokenSlot);

            md.Add(token);
            return View(md);
        }

        private static void SendOrderMessage(String toName, String subject, String body, String destination,String td)
        {

            var fromAddress = new MailAddress("Enter Your Email ID", "Enter Name");
            var toAddress = new MailAddress(destination, "Customer");
            const string fromPassword = "Password";
            

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
                Body = body+td.ToString()

            })
            {
                smtp.Send(message);
            }


        }
    }
}