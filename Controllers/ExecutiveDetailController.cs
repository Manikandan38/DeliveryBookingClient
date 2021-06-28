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
    public class ExecutiveDetailController : Controller
    {

        //Base url of API.
        string Baseurl = "https://localhost:44331/";


        //Function for Get list of Executive.
        public async Task<IActionResult> GetAllExecutive()
        {
            List<ExecutiveDetail> ExecutiveInfo = new List<ExecutiveDetail>();

            try
            {
                using (var client = new HttpClient())
                {

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
            }
            catch (Exception)
            {

                ViewBag.errormsg = "Could not get response from API. Check the url!";
            }
            return View(ExecutiveInfo);

        }


        //Function for Register new Executive.
        public async Task<IActionResult> Create()
        {
            try
            {
                List<City> c = new List<City>();
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

                ViewData["city"] = new SelectList(c, "CityID", "CityName");
            }
            catch (Exception)
            {

                ViewBag.errormsg = "Could not get response from API. Check the url!";
            }

            return View();
        }


        //POST : Register
        [HttpPost]
        public async Task<IActionResult> Create(ExecutiveDetail C)
        {
            try
            {
                ExecutiveDetail Eobj = new ExecutiveDetail();
                //  HttpClient obj = new HttpClient();
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(Baseurl);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(C), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("api/ExecutiveDetails/PostExecutiveDetail", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Eobj = JsonConvert.DeserializeObject<ExecutiveDetail>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "Could not get response from API. Check the url!";
            }

            return RedirectToAction("ExecutiveLogin", "Login");
        }


        //Function for Edit Executive Details.
        public async Task<IActionResult> Edit(int id)
        {
            ExecutiveDetail E = new ExecutiveDetail();
            try
            {

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/ExecutiveDetails/PutExecutiveDetail/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        E = JsonConvert.DeserializeObject<ExecutiveDetail>(apiResponse);
                    }
                }
                
            }
            catch (Exception)
            {

                ViewBag.errormsg = "Could not get response from API. Check the url!";
            }

            return View(E);
        }


        //POST : Edit.
        [HttpPost]
        public async Task<IActionResult> Edit(ExecutiveDetail E)
        {
            ExecutiveDetail E1 = new ExecutiveDetail();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    int id = E.ExecutiveID;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(E), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:44331/api/ExecutiveDetails/PutExecutiveDetail/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = "Success";
                        E1 = JsonConvert.DeserializeObject<ExecutiveDetail>(apiResponse);
                    }
                }
                
            }
            catch (Exception)
            {
                ViewBag.errormsg = "Could not get response from API. Check the url!";
            }

            return RedirectToAction("GetAllExecutive");
        }


        //Function for Delete Executive 
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            ExecutiveDetail E = new ExecutiveDetail();
            try
            {
                TempData["ExecutiveID"] = id;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/ExecutiveDetails/DeleteExecutiveDetail/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        E = JsonConvert.DeserializeObject<ExecutiveDetail>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {

                ViewBag.errormsg = "Could not get response from API. Check the url!";
            }

            return View(E);
        }


        //POST : Delete
        [HttpPost]
        public async Task<ActionResult> Delete(UserDetail U)
        {
            try
            {
                int ExecutiveID = Convert.ToInt32(TempData["ExecutiveID"]);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44331/api/ExecutiveDetails/DeleteExecutiveDetail/" + ExecutiveID))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                
            }
            catch (Exception)
            {

                ViewBag.errormsg = "Could not get response from API. Check the url!";
            }

            return RedirectToAction("GetAllExecutive");
        }
    }
}
