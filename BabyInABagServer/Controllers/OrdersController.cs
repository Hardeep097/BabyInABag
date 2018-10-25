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
        public ActionResult Edit([Bind(Include = "Order_Id,Order_Date_Placed,Order_Status,Order_Details,Order_Date_Paid,Invoice_Status,CustomerId")] Order order)
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

                Create(activeCart);

                return View(activeCart);
            }
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create(List<Product> activeProducts)
        {
            List<Product> activeCart = activeProducts;

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

            Order order = new Order();
            order.Customer_Id = customer_id;
            order.Shipping_Address = "32 Mill Street South Brampton On L6Y 1S6";
            order.Products = activeCart;
            order.Order_Status = order_status.Submitted;
            order.Order_Date_Placed = System.DateTime.Now;
            order.Order_Date_Paid = System.DateTime.Now;
            order.Invoice_Status = "Paid";

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
            var getData = new GetPayPalData();
            string response = getData.GetPayPalResponse(Request.QueryString["tx"]);
            
            Regex rgx = new Regex(@"\b receiver_email=\K[\S]*");
            string receiver_email;

            try
            {
                receiver_email = rgx.Match(response).ToString();
            }
            catch(Exception e)
            {
                receiver_email = "Caught Error";
            }
            

            ViewBag.txt = receiver_email;
            return View();
        }

    }
}
