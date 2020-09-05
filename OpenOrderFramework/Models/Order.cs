using OpenOrderFramework.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OpenOrderFramework.Models
{
    [Bind(Exclude = "OrderId")]
    public partial class Order
    {

        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(70)]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(40)]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [DisplayName("Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(40)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(24)]
        public string Phone { get; set; }

        [Display(Name = "Experation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Experation { get; set; }

        [Display(Name = "Credit Card")]
        [NotMapped]
        [Required]
        [DataType(DataType.CreditCard)]
        public String CreditCard { get; set; }

        [Display(Name = "Credit Card Type")]
        [NotMapped]
        public String CcType { get; set; }

        public bool SaveInfo { get; set; }


        [DisplayName("Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public String Slot { get; set; }

        public string ToString(Order order)
        {
            StringBuilder bob = new StringBuilder();

            
            

            bob.Append("Order Information for Order: "+ order.OrderId +" Placed at: " + order.OrderDate +" ").AppendLine();
            bob.Append(" Name: " + order.FirstName + " " + order.LastName + " ");
            bob.Append("Address: " + order.Address + " " + order.City + " " + order.State + " " + order.PostalCode + " ");
            bob.Append("Contact: " + order.Email + "     " + order.Phone + " ");
            bob.Append("Card Info: " + order.CreditCard + " " + order.Experation.ToString("dd-MM-yyyy") + " ");
            //  bob.Append("Credit Card Type: " + order.CcType + " ");
            
            bob.Append("                                               ").AppendLine();
            bob.Append("     ").AppendLine();
            // Display header 
            //string header = "Item Name " + " Quantity " + " Price ";
            //bob.Append(header).AppendLine();

            String output = String.Empty;
            try
            {
                foreach (var item in order.OrderDetails)
                {
                    bob.Append("");
                    output = "" + item.Item.Name + " " + " " + item.Quantity + " " + " " + item.Quantity * item.UnitPrice + " ";
                    bob.Append(output).AppendLine();
                    Console.WriteLine(output);
                    bob.Append(" ");
                   
                }
            }
            catch (Exception )
            {
                output = "No items ordered.";
            }
            bob.Append(" ");
            bob.Append(" ");
            // Display footer 
            string footer = String.Format("{0,-12}{1,12}\n",
                                          "Total", order.Total);
            bob.Append(footer).AppendLine();
            bob.Append(" ");
            bob.Append("Token Details: Pick your order on below mentioned time. ");
            return bob.ToString();
        }
    }
}