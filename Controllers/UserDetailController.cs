using DeliveryBookingCllient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.Controllers
{
    public class UserDetailController : Controller
    {
        //Base url for API
        string Baseurl = "https://localhost:44331/";



        //Function for Getting all User.
        public async Task<IActionResult> GetAllUser()
        {
            List<UserDetail> UserInfo = new List<UserDetail>();
            try
            {

                using (var client = new HttpClient())
                {

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
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }
            return View(UserInfo);
        }


        //Function for Register new User.
        public async Task<IActionResult> Create()
        {
            List<City> c = new List<City>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("api/Cities/GetAllCity");

                    if (Res.IsSuccessStatusCode)
                    {
                        var UserDetailResponse = Res.Content.ReadAsStringAsync().Result;
                        c = JsonConvert.DeserializeObject<List<City>>(UserDetailResponse);

                    }

                }

                ViewData["city"] = new SelectList(c,"CityID","CityName");

            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }
            return View();
        }


        //POST : Register.
        [HttpPost]
        public async Task<IActionResult> Create(UserDetail C)
        {
            
            try
            {
                UserDetail Uobj = new UserDetail();
                //  HttpClient obj = new HttpClient();
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(Baseurl);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(C), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("api/UserDetails/PostUserDetail", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Uobj = JsonConvert.DeserializeObject<UserDetail>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return RedirectToAction("UserLogin", "Login");
        }


        //Function for Edit the User
        public async Task<IActionResult> Edit(int id)
        {
            UserDetail C = new UserDetail();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/UserDetails/PutUserDetail/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        C = JsonConvert.DeserializeObject<UserDetail>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return View(C);
        }


        //POST : Edit
        [HttpPost]
        public async Task<IActionResult> Edit(UserDetail U)
        {
            try
            {
                UserDetail U1 = new UserDetail();
                using (var httpClient = new HttpClient())
                {
                    int id = U.UserID;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(U), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:44331/api/UserDetails/PutUserDetail/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = "Success";
                        U1 = JsonConvert.DeserializeObject<UserDetail>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }
            return RedirectToAction("GetAllUser");
        }


        //Function for Delete the Existing User.
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            UserDetail U = new UserDetail();
            try
            {
                TempData["UserID"] = id;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/UserDetails/DeleteUserDetail/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        U = JsonConvert.DeserializeObject<UserDetail>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return View(U);
        }


        //POST : Delete.
        [HttpPost]
        public async Task<ActionResult> Delete(UserDetail U)
        {
            try
            {
                int UserID = Convert.ToInt32(TempData["UserID"]);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44331/api/UserDetails/DeleteUserDetail/" + UserID))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }
            return RedirectToAction("GetAllUser");
        }
    }
}
