using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BabyInABagServer.Models;
using BabyInABagServer.Services;
using System.IO;
using System.Collections;
using BabyInABagServer.Models.VMs;
using PayPal;

namespace BabyInABagServer.Controllers
{
    public class ProductsController : Controller
    {
        private Context db = new Context();
        List<CartItem> cart = new List<CartItem>();


        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        //// GET: Products/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_Id,Product_Name,Product_Price,Product_Description,Product_Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ActionResult Products()
        {

            var categories = db.ProductCategories.ToList();
            if (categories != null)
            {
                ViewBag.data = categories;
            }


            return View(db.Products.ToList());
        }
        public ActionResult CustomizeProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory pc = db.ProductCategories.Find(id);
            
            return View(pc);
        }
        
        [HttpPost]
        public ActionResult CustomizeProduct(ProductCategory pcat, FormCollection frm)
        {
            Product product = new Product
            {
                Product_Name = "Custom: " + pcat.Product_Category,
                Product_Description = "This is a custom made product made by customer " + Session["username"],
                Product_Price = pcat.Default_Price,
                Product_Category_Id = pcat.Product_Category_Id,
                Active = false,
                Knit_Type = frm["knit"],
                Color = frm["color"],
                Product_Image = pcat.Default_Image
            };
            if (frm["additionalRequirements"] != "")
            {
                product.AdditionalRequirements = frm["additionalRequirements"];
            }

            using (db)
            {
                db.Products.Add(product);
                db.SaveChanges();
            }


            return RedirectToAction("AddToCart", new { id = product.Product_Id });
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Product product = db.Products.Find(id);

            Product product = db.Products.Find(id);
            TempData["imagePath"] = product.Product_Image;

            if (product == null)
            {
                return HttpNotFound();
            }
            var categories = db.ProductCategories.ToList();

            if (categories != null)
            {
                ViewBag.data = categories;
            }
            
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_Id,Product_Name,Product_Price,Product_Description,Product_Image,Active,Size,Product_Category_Id,ImageFile")] Product product)
        {

            if (product.ImageFile == null)
            {
                product.Product_Image = TempData["imagePath"].ToString();
                using (db)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ManageProducts", "Admin", new { area = "" });
                }
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);

                if (extension.Equals(".jpg") || extension.Equals(".png") || extension.Equals(".jpeg"))
                {
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    product.Product_Image = "/images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                    product.ImageFile.SaveAs(fileName);

                    using (db)
                    {
                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ManageProducts", "Admin", new { area = "" });
                    }
                }
                else
                {
                    String error = "You may only submit JPG or PNG";
                    ViewBag.error = error;
                }
            }
            //ModelState.Clear();

            return View(product);
        }

        //// GET: Products/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Product product = db.Products.Find(id);
        //    db.Products.Remove(product);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(FormCollection frm)
        {
            int id = Int32.Parse(frm["pid"]);
            int quan = Int32.Parse(frm["quantity"]);
            
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            if(Session["username"] == null)
            {
                return RedirectToAction("Login","Login", null);
            }
            else
            {
                if (TempData["cart"] == null)
                {
                    
                    CartItem item = new CartItem((int)id, (int)quan);
                    cart.Add(item);
                    TempData["cart"] = cart;
                    Session["cart"] = cart;
                }
                else
                {
                    CartItem item = new CartItem((int)id, (int)quan);
                    cart = TempData["cart"] as List<CartItem>;
                    cart.Add(item);
                    Session["cart"] = cart;
                }
            }
            return RedirectToAction("Cart","Cart",null);
        }
    }
}
