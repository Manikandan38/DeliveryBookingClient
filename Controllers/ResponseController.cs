using DeliveryBookingCllient.Data;
using DeliveryBookingCllient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.Controllers
{
    public class ResponseController : Controller
    {
        public readonly DeliveryBookingClientContext _context;

        public ResponseController(DeliveryBookingClientContext context)
        {
            _context = context;
        }

        //Base url for API
        string Baseurl = "https://localhost:44331/";


        //Function for Viewing the Accepted Responses.
        public async Task<IActionResult> ViewAcceptedResponses()
        {
            List<ExecutiveDetail> ExecutiveInfo = new List<ExecutiveDetail>();
            List<RequestAccepted> ra = new List<RequestAccepted>();

            try
            {
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

                }


                ExecutiveDetail obj1 = (from i in ExecutiveInfo
                                        where i.UserName == HttpContext.Session.GetString("Name")
                                        select i).FirstOrDefault();


                ViewData["ExecName"] = obj1.Name;
                ra = (from i in _context.RequestAccepted
                          where i.ExecutiveID == obj1.ExecutiveID
                          select i).ToList();

            }
            catch (Exception)
            {
                ViewBag.errormsg = "Couldn't fetch some data!";
            }

            return View(ra.ToList());
        }


        //Function for Viewing the Rejected Responses.
        public IActionResult ViewRejectedResponses(int userid)
        {
            List<RequestRejected> ra = new List<RequestRejected>();
            try
            {
                ra = (from i in _context.RequestRejected
                          where i.UserID == userid
                          select i).ToList();

            }
            catch (Exception)
            {
                ViewBag.errormsg = "Couldn't fetch some data!";
            }

            return View(ra.ToList());
        }


        //Function for View All Executive Responses.
        public async Task<IActionResult> ViewAllResponses(int userid)
        {
            try
            {

                var ra = (from i in _context.RequestAccepted
                          where i.UserID == userid
                          select i);

                var rr = (from i in _context.RequestRejected
                          where i.UserID == userid
                          select i);

                dynamic mymodel = new ExpandoObject();
                mymodel.AcceptedList = ra.ToList();
                mymodel.RejectedList = rr.ToList();

                List<UserDetail> UserDetailInfo = new List<UserDetail>();

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

                UserDetail ud = (from i in UserDetailInfo
                                 where i.Name == HttpContext.Session.GetString("Name")
                                 select i).FirstOrDefault();

                ExecutiveDetail ed = (from i in ExecutiveInfo
                                      where i.CityID == ud.CityID
                                      select i).FirstOrDefault();

                ViewData["Username"] = ud.Name;
                ViewData["Executivename"] = ed.Name;

                return View(mymodel);

                

            }
            catch (Exception)
            {

                throw;
            }
        }


        //Function for Getting all Executive Response belongs to the User id.
        public async Task<IActionResult> GetAllResponse()
        {
            List<ExecutiveResponse> ResponseInfo = new List<ExecutiveResponse>();
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("api/EResponses/GetAllResponse");

                    if (Res.IsSuccessStatusCode)
                    {
                        var RResponse = Res.Content.ReadAsStringAsync().Result;
                        ResponseInfo = JsonConvert.DeserializeObject<List<ExecutiveResponse>>(RResponse);

                    }
                }

            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return View(ResponseInfo);
        }


        //Function for Crete an Response.
        public IActionResult Create(int reqid, int userid)
        {
            try
            {
                ViewData["ReqID"] = reqid;
                TempData["ReqID"] = reqid;
                TempData["UserID"] = userid;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }


        //POST : Create Response
        [HttpPost]
        public async Task<IActionResult> Create(ExecutiveResponse C)
        {
            try
            {
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

                ExecutiveResponse Cobj = new ExecutiveResponse();
                //  HttpClient obj = new HttpClient();
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(Baseurl);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(C), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("api/EResponses/PostResponse", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Cobj = JsonConvert.DeserializeObject<ExecutiveResponse>(apiResponse);
                    }
                }

                ExecutiveDetail obj1 = (from i in ExecutiveInfo
                                        where i.UserName == HttpContext.Session.GetString("Name")
                                        select i).FirstOrDefault();
                TempData["ExecID"] = obj1.ExecutiveID;

                if (!C.status)
                {
                    RequestRejected r = new RequestRejected();
                    r.RequestID = Convert.ToInt32(TempData["ReqID"]);
                    r.UserID = Convert.ToInt32(TempData["UserID"]);
                    r.ExecutiveID = obj1.ExecutiveID;
                    _context.RequestRejected.Add(r);
                    _context.SaveChanges();

                    int ReqID = Convert.ToInt32(TempData["ReqID"]);
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.DeleteAsync("https://localhost:44331/api/UserRequests/DeleteUserRequest/" + ReqID))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }

                }
                else
                {

                    RequestAccepted r = new RequestAccepted();
                    r.RequestID = Convert.ToInt32(TempData["ReqID"]);
                    r.UserID = Convert.ToInt32(TempData["UserID"]);
                    r.ExecutiveID = obj1.ExecutiveID;
                    _context.RequestAccepted.Add(r);
                    _context.SaveChanges();



                    int ReqID = Convert.ToInt32(TempData["ReqID"]);
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.DeleteAsync("https://localhost:44331/api/UserRequests/DeleteUserRequest/" + ReqID))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
                    return RedirectToAction("AddtoStatus", new
                    {
                        Reqid = r.RequestID
                    });

                }

            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return RedirectToAction("ResponseToRequest", "Response");
        }


        //Function for Updating the Status of Response.
        public IActionResult AddToStatus(int Reqid)
        {
            try
            {
                DeliveryStatus ds = new DeliveryStatus();

                RequestAccepted ra = (from i in _context.RequestAccepted
                                      where i.RequestID == Reqid
                                      select i).FirstOrDefault();

                ds.RequestID = Reqid;
                ds.ExecutiveID = ra.ExecutiveID;
                ds.UserID = ra.UserID;
                ds.Received = false;
                ds.Shipped = false;
                ds.Delivered = false;

                _context.DeliveryStatus.Add(ds);
                _context.SaveChanges();

                return RedirectToAction("ResponseToRequest", "Response");
            }
            catch (Exception)
            {

                throw;
            }
        }


        //Function for Edit the Response.
        public async Task<IActionResult> Edit(int id)
        {
            ExecutiveResponse C = new ExecutiveResponse();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/EResponses/PutResponse/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        C = JsonConvert.DeserializeObject<ExecutiveResponse>(apiResponse);
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
        public async Task<IActionResult> Edit(ExecutiveResponse C)
        {
            try
            {
                ExecutiveResponse C1 = new ExecutiveResponse();
                using (var httpClient = new HttpClient())
                {
                    int id = C.ResponseID;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(C), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:44331/api/EResponses/PutResponse/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = "Success";
                        C1 = JsonConvert.DeserializeObject<ExecutiveResponse>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }
            return RedirectToAction("GetAllResponse");
        }


        //Function for Delete the Response
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            ExecutiveResponse c = new ExecutiveResponse();
            try
            {
                TempData["ResponseID"] = id;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44331/api/EResponses/Response/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<ExecutiveResponse>(apiResponse);
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return View(c);

        }


        //POST : Delete
        [HttpPost]
        public async Task<ActionResult> Delete(ExecutiveResponse C)
        {
            try
            {
                int ResponseID = Convert.ToInt32(TempData["ResponseID"]);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44331/api/EResponses/DeleteResponse/" + ResponseID))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return RedirectToAction("GetAllResponse");
        }


        //Function for Response to User's Request.
        public async Task<ActionResult> ResponseToRequest()
        {
            try
            {
                List<UserRequest> UserRequestInfo = new List<UserRequest>();
                List<ExecutiveDetail> ExecutiveInfo = new List<ExecutiveDetail>();
                List<UserDetail> UserDetailInfo = new List<UserDetail>();
                List<ExecutiveResponse> ResponseInfo = new List<ExecutiveResponse>();

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
                    //return View(UserRequestInfo);
                }

                //Response list
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("api/Responses/GetAllResponse");

                    if (Res.IsSuccessStatusCode)
                    {
                        var UserDetailResponse = Res.Content.ReadAsStringAsync().Result;
                        ResponseInfo = JsonConvert.DeserializeObject<List<ExecutiveResponse>>(UserDetailResponse);

                    }
                    //return View(UserRequestInfo);
                }

                ExecutiveDetail obj1 = (from i in ExecutiveInfo
                                        where i.UserName == HttpContext.Session.GetString("Name")
                                        select i).FirstOrDefault();


                var obj2 = (from i in UserRequestInfo
                            where i.ExecutiveID == obj1.ExecutiveID
                            select i);

                ViewData["ExecName"] = obj1.Name;


                if (obj2 != null)
                {
                    return View(obj2);
                }
                else
                {
                    return View();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        //Function for Viewing the Current Delivery status of Request.
        public async Task<IActionResult> ViewStatus(int reqid)
        {
            try
            {
                var obj = _context.DeliveryStatus.ToList();

                DeliveryStatus ra = (from i in obj
                                     where i.RequestID == reqid
                                     select i).FirstOrDefault();

                List<UserDetail> UserDetailInfo = new List<UserDetail>();

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

                }

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

                }



                UserDetail ud = (from i in UserDetailInfo
                                 where i.UserID == ra.UserID
                                 select i).FirstOrDefault();

                ExecutiveDetail ed = (from i in ExecutiveInfo
                                      where i.ExecutiveID == ra.ExecutiveID
                                      select i).FirstOrDefault();

                ViewData["username"] = ud.Name;
                ViewData["executivename"] = ed.Name;
                ViewBag.ra = ra;
            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Response from the Server!";
            }

            return View();
        }


        //Function for updating the Accepted Resquests.
        public IActionResult UpdateAcceptedRequests(int reqid)
        {
            try
            {
                var obj = _context.RequestAccepted.ToList();

                RequestAccepted r = (from i in obj
                                     where i.RequestID == reqid
                                     select i).FirstOrDefault();

                ViewBag.ra = r;
            }
            catch (Exception)
            {
                ViewBag.errormsg = "Could not fetch some data!";
            }
            return View();
        }


        //POST : Update 
        [HttpPost]
        public IActionResult UpdateAcceptedRequests([Bind("DeliveryID,UserID,RequestID,ExecutiveID,Received,Shipped,Delivered")] DeliveryStatus d)
        {
            try
            {
                DeliveryStatus ds = (from i in _context.DeliveryStatus
                                     where i.RequestID == d.RequestID
                                     select i).FirstOrDefault();

                if (ds != null)
                {
                    _context.DeliveryStatus.Remove(ds);
                    _context.SaveChanges();
                }

                _context.DeliveryStatus.Add(d);
                _context.SaveChanges();

            }
            catch (Exception)
            {
                ViewBag.errormsg = "No Data Found!";
            }

            return RedirectToAction("ViewAcceptedResponses", "Response");
        }
    
    }
}
