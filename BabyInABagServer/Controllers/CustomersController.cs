using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BabyInABagServer.Models;
using BabyInABagServer.Models.VMs;
using BabyInABagServer.Models.Repos;
using System.Web.Routing;
using System.Web.Http.Description;
using BabyInABagServer.Services;

namespace BabyInABagServer.Controllers
{
    public class CustomersController : Controller
    {
        private Context db = new Context();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseType(typeof(CustomerAdminSignup))]
        public ActionResult Create(CustomerAdminSignup cas)
        {
            if (ModelState.IsValid)
            {
                CustomerRepository cr = new CustomerRepository();
                Customer customer = cr.GetCustomer(cas);

                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("CreateVerify");
            }
            return View();
        }

        public ActionResult CreateVerify()
        {
            return View();
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Customer_Id,First_Name,Middle_Name,Last_Name,Customer_Phone,Customer_Email,Customer_Details,Username,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public ActionResult CustomerOrder(int? Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Customer c = db.Customers.Find(Id);

            if (c == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var orderId = (from Order in db.Orders
                    where Order.Customer_Id == c.Customer_Id
                    select Order.Order_Id).First();

            Order o = db.Orders.Find(orderId);

            CustomerOrder co = new CustomerOrder
            {
                Order = o
            };

            foreach (Product p in db.Products.ToList())
            {
                CustomerProduct cp = new CustomerProduct
                {
                    Id = p.Product_Id,
                    isAdded = o.Products.Contains(p),
                    ProductInfo = $"{p.Product_Name} ({p.Product_Description})"
                };

                co.CustomerProducts.Add(cp);
            }
            return View(co);
        }

        [HttpPost]
        public ActionResult CustomerOrder(CustomerOrder co)
        {
            if (ModelState.IsValid)
            {
                Order o = db.Orders.Find(co.Order.Order_Id);
                db.Entry(o).State = System.Data.Entity.EntityState.Modified;

                foreach (CustomerProduct cp in co.CustomerProducts)
                {
                    Product p = db.Products.Find(cp.Id);

                    if (cp.isAdded)
                        o.Products.Add(p);
                    else
                        o.Products.Remove(p);
                }
                db.SaveChanges();

                var customerId = (from Order in db.Orders
                              where Order.Order_Id == o.Order_Id
                              select Order.Customer_Id).First();

                return RedirectToAction("CurrentOrder", new { Id = o.Order_Id });
            }
            else
                return View(co);
        }

        public ActionResult CurrentOrder(int? id)
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

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
    }
}
