using BabyInABagServer.Models;
using BabyInABagServer.Models.VMs;
using BabyInABagServer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                            products[c].Quantity = currentCart[d].Quantity;
                            activeCart.Add(products[c]);

                            for(int i = 0; i < products[c].Quantity;i++)
                            {
                                subtotalPrice += products[c].Product_Price;
                                subtotalAmount++;
                            }
                            
                        }
                    }
                }
                ViewBag.Subtotal = "Subtotal (" + subtotalAmount + " item): CDN$ " + subtotalPrice;
                return View(activeCart);
            }
            return View();
        }

        public ActionResult RemoveFromCart(int? id)
        {
            List<CartItem> currentCart = (List<CartItem>)Session["cart"];
            CartItem cartItem = new CartItem();
            for(int i = 0; i < currentCart.Count; i++)
            {
                if (currentCart[i].ProductID == (int)id)
                {
                    cartItem = currentCart[i];
                }
            }
            currentCart.Remove(cartItem);
            Session["cart"] = currentCart;
            return RedirectToAction("Cart", "Cart", null);
        }
    }
}