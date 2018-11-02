using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BabyInABagServer.Models;
using BabyInABagServer.Models.VMs;
using BabyInABagServer.Services;

namespace BabyInABagServer.Controllers
{
    public class OrdersController : Controller
    {
        private Context db = new Context();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Customer_Id", "First_Name", order.Customer_Id);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Order_Id,Order_Date_Placed,Order_Status,Order_Details,Order_Date_Paid,Invoice_Status,Customer_Id")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Customer_Id", "First_Name", order.Customer_Id);
            return View(order);
        }

        public ActionResult Checkout()
        {
            if (Session["cart"] == null)
            {
                ViewBag.empty = "Your cart is empty";
            }
            else
            {
                List<CartItem> currentCart = (List<CartItem>)Session["cart"];
                List<Product> activeCart = new List<Product>();
                List<Product> products = new List<Product>();
                decimal subtotalPrice = 0;
                int subtotalAmount = 0;

                products = db.Products.ToList();

                for (int c = 0; c < products.Count; c++)
                {
                    for (int d = 0; d < currentCart.Count; d++)
                    {
                        if (products[c].Product_Id.Equals(currentCart[d].ProductID))
                        {
                            activeCart.Add(products[c]);
                            subtotalPrice += products[c].Product_Price;
                            subtotalAmount++;
                        }
                    }
                }
                ViewBag.Subtotal = "Subtotal (" + subtotalAmount + " item): CDN$ " + subtotalPrice;
                ViewBag.Subtotalprice = subtotalPrice;

               // Create(activeCart);

                return View(activeCart);
            }
            return View();
        }

        public List<Product> GetCartProducts()
        {
            List<CartItem> currentCart = (List<CartItem>)Session["cart"];
            List<Product> activeCart = new List<Product>();
            List<Product> products = new List<Product>();

            products = db.Products.ToList();

            for (int c = 0; c < products.Count; c++)
            {
                for (int d = 0; d < currentCart.Count; d++)
                {
                    if (products[c].Product_Id.Equals(currentCart[d].ProductID))
                    {
                        activeCart.Add(products[c]);
                    }
                }
            }

            return activeCart;
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create(Order order)
        {
            //List<Product> activeCart = activeProducts;

            string username = (string)Session["username"];
            int customer_id = 0;
            List<Customer> customers = db.Customers.ToList();
            for(int i = 0; i < customers.Count; i++)
            {
                if(customers[i].Username == username)
                {
                    customer_id = customers[i].Customer_Id;
                }
            }

            //Order order = new Order();
            order.Customer_Id = customer_id;
            order.Products = GetCartProducts();
            order.Order_Status = Order_Status.Submitted;
            order.Order_Date_Placed = System.DateTime.Now;
           

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
            }
        }

        public ActionResult Payment()
        {
            return View();
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetPayPalData()
        {
            //POST Response to PayPal with TX token, pull Order Response for SUCCESS or FAIL payment
            var getData = new GetPayPalData();
            string response = getData.GetPayPalResponse(Request.QueryString["tx"]);

            Regex payment_success_rgx = new Regex(@"SUCCESS");
            if(payment_success_rgx.Match(response).ToString().Equals("SUCCESS"))
            {
                //Regex to Parse through decoded response
                Regex address_name_rgx = new Regex(@"(?<=address_name=).*?(?=\s)");
                Regex address_street_rgx = new Regex(@"(?<=address_street=).*?(?=\s)");
                Regex address_city_rgx = new Regex(@"(?<=address_city=).*?(?=\s)");
                Regex address_country_rgx = new Regex(@"(?<=address_country=).*?(?=\s)");
                Regex address_state_rgx = new Regex(@"(?<=address_state=).*?(?=\s)");
                Regex address_zip_rgx = new Regex(@"(?<=address_zip=).*?(?=\s)");
                Regex payment_gross_rgx = new Regex(@"(?<=payment_gross=).*?(?=\s)");

               //Parsed Fields being Decoded
                string address_street_decoded = HttpUtility.UrlDecode(address_street_rgx.Match(response).ToString());
                string address_name_decoded = HttpUtility.UrlDecode(address_name_rgx.Match(response).ToString());
                string address_city_decoded = HttpUtility.UrlDecode(address_city_rgx.Match(response).ToString());
                string address_country_decoded = HttpUtility.UrlDecode(address_country_rgx.Match(response).ToString());
                string address_state_decoded = HttpUtility.UrlDecode(address_state_rgx.Match(response).ToString());
                string address_zip_decoded = HttpUtility.UrlDecode(address_zip_rgx.Match(response).ToString());
                string payment_gross_decoded = HttpUtility.UrlDecode(payment_gross_rgx.Match(response).ToString());

                String shipping_address = address_street_decoded + "\n" +
                                          address_city_decoded + ", "  + 
                                          address_state_decoded + "\n" +
                                          address_country_decoded + ", " + 
                                          address_zip_decoded;

                Order order = new Order
                {
                    Shipping_Address = shipping_address,
                    Full_Name = address_name_decoded,
                    Order_Total = Convert.ToDecimal(payment_gross_decoded)
                };

                Create(order);

                ViewBag.status_message = "Your Payment was Successful!";
            }
            else
            {
                ViewBag.status_message = "Your Payment didnt go through!";
            }
            return View();
        }
    }
}
