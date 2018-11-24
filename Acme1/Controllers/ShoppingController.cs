using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Acme1.Models;
using System.IO;
using System.Text.RegularExpressions;
using Acme1.Models.ViewModels;

namespace Acme1.Controllers
{
    public class ShoppingController : Controller
    {

        SqlConnection dbcon = new SqlConnection(ConfigurationManager.ConnectionStrings["Acmedb"].ConnectionString.ToString());

        // GET: Shopping
        public ActionResult Index(string id = "")
        {
            if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
            {
                string whereclause = " where categoryid = '" + id + "'";
                ViewBag.category = id;
                try
                {
                    dbcon.Open();
                    List<Product> prodlist = Product.GetProductList(dbcon, whereclause);
                    dbcon.Close();
                    if (prodlist.Count() > 0) return View(prodlist);
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data in Shopping page";
            return View("error");
        }


        // GET: Shopping
        public ActionResult Order(string id = "")
        {
            if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
            {
                string whereclause = " where categoryid = '" + id + "'";
                ViewBag.category = id;
                try
                {
                    dbcon.Open();
                    Product product = Product.GetProductSingle(dbcon, id);
                    dbcon.Close();
                    if (product.ProductID != null)
                    {
                        return View(product);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }


            }

            ViewBag.errormsg = "Invalid data in the Order Page";
            return View("_Error");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order(Cart_Lineitem cart)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon.Open();
                    cart.CartNumber = 100; //use Session["cartnumber"] later
                    int intresult = Cart_Lineitem.CartUpSert(dbcon, cart);
                    dbcon.Close();
                    return RedirectToAction("Cart");
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data found in Order Page";
            return View("error");
        }

        public ActionResult Cart()
        {
            try
            {
                dbcon.Open();
                List<Cartvm1> cart = Cartvm1.GetCartList(dbcon, 100);
                dbcon.Close();
                return View(cart);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }






}