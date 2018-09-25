using BabyInABagServer.Models;
using BabyInABagServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace BabyInABagServer.Controllers
{
    //admin
    public class AdminController : Controller
    {
        private Context db = new Context();

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Accounts()
        {
            return View();
        }
        
        public ActionResult AddProduct()
        {
            var categories = db.ProductCategories.ToList();

           if(categories != null)
            {
                ViewBag.data = categories;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product, FormCollection frm)
        {
            String categoryChoice = frm["choice"].ToString();
            
            if (categoryChoice.Equals("Create new"))
            {

                ProductCategory product_category = new ProductCategory();
                product_category.Product_Category = frm["Product_Category_new"].ToString();
                
                    db.ProductCategories.Add(product_category);
                    db.SaveChanges();

                    TempData["category_id"] = product_category.Product_Category_Id;
                
            }
            //getting product category
            var categories = db.ProductCategories.ToList();

            string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
            string extension = Path.GetExtension(product.ImageFile.FileName);

            if(extension.Equals(".jpg",StringComparison.OrdinalIgnoreCase) || extension.Equals(".png",  StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
            { 
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            product.Product_Image = "/images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
            product.ImageFile.SaveAs(fileName);
            product.Active = true;
            product.Size = "Standard";

                if (TempData["category_id"] != null)
                {
                    product.Product_Category_Id = (int)TempData["category_id"];
                }
           
                using (db)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }
            else
            {
                String error = "You may only submit JPG or PNG";
                ViewBag.error = error;
            }
            ModelState.Clear();

            if (categories != null)
            {
                ViewBag.data = categories;
            }

            return View();

        }

        public ActionResult ManageProducts()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Orders()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult Report1()
        {
            return View();
        }

        public ActionResult Report2()
        {
            return View();
        }

        public ActionResult Report3()
        {
            return View();
        }
    }
}