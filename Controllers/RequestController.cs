using DeliveryBookingCllient.Models;
using Microsoft.AspNetCore.Http;
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
    public class RequestController : Controller
    {
        //Base url of API
        string Baseurl = "https://localhost:44331/";


        //Function for Getting All Requests.
        public async Task<IActionResult> GetAllRequest()
        {
            List<UserRequest> UserRequestInfo = new List<UserRequest>();

            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("api/UserRequests/GetAllRequest");

                    if (Res.IsSuccessStatusCode)
                    {
                        var UserRequestResponse = Res.Content.ReadAsStringAsync().Result;
                        UserRequestInfo = JsonConvert.DeserializeObject<List<UserRequest>>(UserRequestResponse);

                    }
                    
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No response from the server!";
            }
            return View(UserRequestInfo);
        }


        //Function for Creating new Request
        public async Task<IActionResult> Create()
        {
            List<UserDetail> UserInfo = new List<UserDetail>();
            List<ExecutiveDetail> ExecInfo = new List<ExecutiveDetail>();

            //Getting User
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

            //Getting Executive.
            using (var client = new HttpClient())
            {
                string Baseurl = "https://localhost:44331/";

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/ExecutiveDetails/GetAllExecutive");

                if (Res.IsSuccessStatusCode)
                {
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    ExecInfo = JsonConvert.DeserializeObject<List<ExecutiveDetail>>(UserResponse);

                }

            }


            //Getting User's Record.
            UserDetail obj1 = (from i in UserInfo
                               where i.UserName == HttpContext.Session.GetString("Name")
                               select i).FirstOrDefault();


            //Getting Executives Record.
            var obj2 = (from i in ExecInfo
                        where i.CityID == obj1.CityID   //Getting Executive who is on User's city.
                        select i);




            if (obj1 != null && obj2 != null)
            {
                //Pick the Random Executive who match with User's City.
                ExecutiveDetail ed = obj2.ElementAt(new Random(DateTime.Now.Millisecond).Next(obj2.Count()));

                ViewBag.Message = obj1;
                ViewData["Address"] = obj1.Address;
                ViewData["Executiveid"] = ed.ExecutiveID;
                ViewData["Executivename"] = ed.Name;
                return View();
            }
            else
            {
                return View();
            }
        }


        //POST : Request
        [HttpPost]
        public async Task<IActionResult> Create(UserRequest R)
        {
            try
            {
                UserRequest Robj = new UserRequest();
                //  HttpClient obj = new HttpClient();
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(Baseurl);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(R), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("api/UserRequests/PostUserRequest", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Robj = JsonConvert.DeserializeObject<UserRequest>(apiResponse);
                    }
                }
                
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No response from the Server!";
            }

            return RedirectToAction("ListAllRequest");
        }


        //Function for List All Requests According to the User id.
        public async Task<IActionResult> ListAllRequest(int id)
        {
            List<UserRequest> UserRequestInfo = new List<UserRequest>();
            List<UserDetail> UserDetailInfo = new List<UserDetail>();
            List<ExecutiveDetail> ExecutiveInfo = new List<ExecutiveDetail>();

            //Executive Detail list
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/ExecutiveDetails/GetAllExecutive");

                if (Res.IsSuccessStatusCode)
                {
                    var UserDetailResponse = Res.Content.ReadAsStringAsync().Result;
                    ExecutiveInfo = JsonConvert.DeserializeObject<List<ExecutiveDetail>>(UserDetailResponse);

                }
                //return View(UserRequestInfo);
            }

            //User Detail list
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/UserDetails/GetAllUser");

                if (Res.IsSuccessStatusCode)
                {
                    var UserDetailResponse = Res.Content.ReadAsStringAsync().Result;
                    UserDetailInfo = JsonConvert.DeserializeObject<List<UserDetail>>(UserDetailResponse);

                }
                //return View(UserRequestInfo);
            }


            //User Request list
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/UserRequests/GetAllRequest");

                if (Res.IsSuccessStatusCode)
                {
                    var UserRequestResponse = Res.Content.ReadAsStringAsync().Result;
                    UserRequestInfo = JsonConvert.DeserializeObject<List<UserRequest>>(UserRequestResponse);

                }

            }

            UserDetail obj1 = (from i in UserDetailInfo
                               where i.UserName == HttpContext.Session.GetString("Name")
                               select i).FirstOrDefault();

            var obj2 = (from i in UserRequestInfo
                        where i.UserID == obj1.UserID
                        select i);




            ViewData["Userid"] = obj1.UserID;
            ViewData["Username"] = obj1.Name;
            if (obj2 != null)
            {
                return View(obj2.ToList());
            }
            else
            {
                return View();
            }
        }


        //Function for Edit the Details of Request.
        public async Task<IActionResult> Edit(int id)
        {
            UserRequest R = new UserRequest();
            try
            {

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/UserRequests/PutUserRequest/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        R = JsonConvert.DeserializeObject<UserRequest>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }
            return View(R);
        }


        //POST : Edit
        [HttpPost]
        public async Task<IActionResult> Edit(UserRequest R)
        {
            UserRequest R1 = new UserRequest();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    int id = R.RequestID;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(R), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:44331/api/UserRequests/PutUserRequest/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = "Success";
                        R1 = JsonConvert.DeserializeObject<UserRequest>(apiResponse);
                    }
                }
            }

            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }
            return RedirectToAction("GetAllRequest");
        }


        //Function for Deleting the Request.
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            UserRequest R = new UserRequest();
            try
            {
                TempData["RequestID"] = id;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/UserRequests/DeleteUserRequest/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        R = JsonConvert.DeserializeObject<UserRequest>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No response from the server!";
            }

            return View(R);
        }


        //POST : Delete
        [HttpPost]
        public async Task<ActionResult> Delete(UserRequest U)
        {
            try
            {
                int RequestID = Convert.ToInt32(TempData["RequestID"]);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44331/api/UserRequests/DeleteUserRequest/" + RequestID))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return RedirectToAction("GetAllRequest");
        }

    }
}
