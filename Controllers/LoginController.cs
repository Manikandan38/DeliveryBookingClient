using DeliveryBookingCllient.Data;
using DeliveryBookingCllient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.Controllers
{
    public class LoginController : Controller
    {
        private readonly DeliveryBookingClientContext _db;
        public LoginController(DeliveryBookingClientContext db)
        {
            _db = db;
        }
        

        //Admin Login Page
        public IActionResult Login()
        {
            return View();
        }


        //POST : Login
        [HttpPost]
        public IActionResult Login(Login l)
        {
            Admin obj = (from i in _db.Admin
                         where i.Name == l.Name && i.Password == l.Password
                         select i).FirstOrDefault();

            if (obj != null)
            {
                string name = obj.Name;
                HttpContext.Session.SetString("Name", name);
                return RedirectToAction("MyUsers", "Admins");
            }
            else
            {
                ViewBag.msg = "Invalid Entry!";
                return View();
            }
        }


        //User Login Page
        public IActionResult UserLogin()
        {
            return View();
        }


        //POST : User Login
        [HttpPost]
        public async Task<IActionResult> UserLogin(Login l)
        {
            List<UserDetail> UserInfo = new List<UserDetail>();

            using (var client = new HttpClient())
            {
                string Baseurl = "https://localhost:44331/";

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/UserDetails/GetAllUser");

                if (Res.IsSuccessStatusCode)
                {
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    UserInfo = JsonConvert.DeserializeObject<List<UserDetail>>(UserResponse);

                }

            }


            UserDetail obj = (from i in UserInfo
                              where i.UserName == l.Name && i.Password == l.Password
                              select i).FirstOrDefault();

            if (obj != null)
            {
                string name = obj.Name;
                HttpContext.Session.SetString("Name", name);
                return RedirectToAction("Create", "Request");
            }
            else
            {
                ViewBag.msg = "Invalid Entry!";
                return View();
            }
        }


        //Executive Login Page
        public IActionResult ExecutiveLogin()
        {
            return View();
        }


        //POST : Executive
        [HttpPost]
        public async Task<IActionResult> ExecutiveLogin(Login l)
        {
            List<ExecutiveDetail> ExecutiveInfo = new List<ExecutiveDetail>();

            using (var client = new HttpClient())
            {
                string Baseurl = "https://localhost:44331/";

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/ExecutiveDetails/GetAllExecutive");

                if (Res.IsSuccessStatusCode)
                {
                    var ExecutiveResponse = Res.Content.ReadAsStringAsync().Result;
                    ExecutiveInfo = JsonConvert.DeserializeObject<List<ExecutiveDetail>>(ExecutiveResponse);

                }

            }


            ExecutiveDetail obj = (from i in ExecutiveInfo
                                   where i.UserName == l.Name && i.Password == l.Password
                                   select i).FirstOrDefault();

            if (obj != null)
            {
                string name = obj.Name;
                HttpContext.Session.SetString("Name", name);
                return RedirectToAction("ResponseToRequest", "Response");
            }
            else
            {
                ViewBag.msg = "Invalid Entry!";
                return View();
            }

        }


    }
}
