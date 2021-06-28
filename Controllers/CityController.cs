using DeliveryBookingCllient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DeliveryBookingCllient.Controllers
{
    public class CityController : Controller
    {
        //Base url for API
        string Baseurl = "https://localhost:44331/";


        //Function for Getting List of All city.
        public async Task<IActionResult> GetAllCity()
        {
            List<City> CityInfo = new List<City>();
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
                        var CityResponse = Res.Content.ReadAsStringAsync().Result;
                        CityInfo = JsonConvert.DeserializeObject<List<City>>(CityResponse);

                    }
                }
                
            }
            catch (Exception ex)
            {

                ViewBag.errormsg = ex.Message;
            }

            return View(CityInfo);
        }


        //Function for Register new City.
        public IActionResult Create()
        {
            return View(); 
        }


        //POST : Register City.
        [HttpPost]
        public async Task<IActionResult> Create(City C)
        {
            try
            {
                City Cobj = new City();
                //  HttpClient obj = new HttpClient();
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(Baseurl);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(C), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("api/Cities/PostCity", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Cobj = JsonConvert.DeserializeObject<City>(apiResponse);
                    }
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.errormsg = ex.Message;
            }

            return RedirectToAction("GetAllCity");
        }



        //Function to Edit Existing City
        public async Task<IActionResult> Edit(int id)
        {

            City C = new City();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/Cities/PutCity/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        C = JsonConvert.DeserializeObject<City>(apiResponse);
                    }
                }
                
            }
            catch (Exception ex)
            {

                ViewBag.errormsg = ex.Message;
            }
            return View(C);

        }



        //POST : Edit City
        [HttpPost]
        public async Task<IActionResult> Edit(City C)
        {
            try
            {
                City C1 = new City();
                using (var httpClient = new HttpClient())
                {
                    int id = C.CityID;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(C), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:44331/api/Cities/PutCity/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = "Success";
                        C1 = JsonConvert.DeserializeObject<City>(apiResponse);
                    }
                }
                
            }
            catch (Exception ex)
            {

                ViewBag.errormsg = ex.Message;
            }

            return RedirectToAction("GetAllCity");
        }


        //Function to Delete City by City ID.
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            City c = new City();
            try
            {
                TempData["CityID"] = id;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/Cities/DeleteCity/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<City>(apiResponse);
                    }
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.errormsg = ex.Message;
            }
            return View(c);

        }


        //POST : Delete.
        [HttpPost]
        public async Task<ActionResult> Delete(City C)
        {
            try
            {
                int CityID = Convert.ToInt32(TempData["CityID"]);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44331/api/Cities/DeleteCity/" + CityID))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.errormsg = ex.Message;
            }

            return RedirectToAction("GetAllCity");
        }
    }
}
