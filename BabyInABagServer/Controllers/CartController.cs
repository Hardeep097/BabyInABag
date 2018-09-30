using BabyInABagServer.Models;
using BabyInABagServer.Models.VMs;
using BabyInABagServer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BabyInABagServer.Controllers
{
    public class CartController : Controller
    {
        Context db = new Context();

        // GET: Cart
        public ActionResult Cart()
        {
            if(Session["cart"] == null)
            {
                ViewBag.empty = "Your cart is empty";
            }
            else
            {
                List<CartItem> cart = (List<CartItem>)Session["cart"];
                List<Product> activeCart = new List<Product>();
                List<Product> products = new List<Product>();

                products = db.Products.ToList();

                for (int c = 0; c < products.Count; c++)
                {
                    for (int d = 0; d < cart.Count; d++)
                    {
                        if(products[c].Product_Id.Equals(cart[d].ProductID))
                        {
                            activeCart.Add(products[c]);
                        }
                    }
                }

                return View(activeCart);

            }

            return View();
        }
    }
}