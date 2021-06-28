using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliveryBookingCllient.Data;
using DeliveryBookingCllient.Models;
using Microsoft.AspNetCore.Http;

namespace DeliveryBookingCllient.Controllers
{
    public class AdminsController : Controller
    {
        private readonly DeliveryBookingClientContext _context;

        public AdminsController(DeliveryBookingClientContext context)
        {
            _context = context;
        }


        //Home page of the Project.
        public ActionResult Home()
        {
            return View();
        }



        //Showing the Type and List of Users to Admin.
        public ActionResult MyUsers()
        {
            ViewBag.msg = HttpContext.Session.GetString("Name");

            if (ViewBag.msg != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }




        //To View User's Data
        public IActionResult ViewUser()
        {
            try
            {
                ViewBag.msg = HttpContext.Session.GetString("Name");

                if (ViewBag.msg != null)
                {
                    return RedirectToAction("GetAllUser", "UserDetail");
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {

                throw;
            }
            

        }




        //To View Executive's Data
        public IActionResult ViewExecutive()
        {
            try
            {
                ViewBag.msg = HttpContext.Session.GetString("Name");

                if (ViewBag.msg != null)
                {
                    return RedirectToAction("GetAllExecutive", "ExecutiveDetail");
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }




        //To View User's Requests
        public IActionResult ViewUserRequest()
        {
            try
            {
                ViewBag.msg = HttpContext.Session.GetString("Name");

                if (ViewBag.msg != null)
                {
                    return RedirectToAction("GetAllRequest", "Request");
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }




        //To View Executive's Data
        public IActionResult ViewExecutiveResponse()
        {
            try
            {
                ViewBag.msg = HttpContext.Session.GetString("Name");
                if (ViewBag.msg != null)
                {
                    return RedirectToAction("GetAllResponse", "Response");
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }



        //Logout Action for Admin.
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Login");
            }
            catch (Exception)
            {

                throw;
            }
             
        }



        //Logout Action for User.
        public IActionResult UserLogout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("UserLogin", "Login");
            }
            catch (Exception)
            {

                throw;
            }
            
        }



        //Logout Action for Executive.
        public IActionResult ExecutiveLogout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("ExecutiveLogin", "Login");
            }
            catch (Exception)
            {

                throw;
            }
            
        }



        // GET: Admins
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Admin.ToListAsync());

            }
            catch (Exception)
            {

                throw;
            }
        }



        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var admin = await _context.Admin
                    .FirstOrDefaultAsync(m => m.AdminID == id);
                if (admin == null)
                {
                    return NotFound();
                }

                return View(admin);
            }
            catch (Exception)
            {

                throw;
            }
            
        }



        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }



        // POST: Admins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminID,Name,Password")] Admin admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(admin);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Login");
                }
                return View(admin);
            }
            catch (Exception)
            {

                throw;
            }
        }



        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var admin = await _context.Admin.FindAsync(id);
                if (admin == null)
                {
                    return NotFound();
                }
                return View(admin);

            }
            catch (Exception)
            {

                throw;
            }
        }



        // POST: Admins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminID,Name,Password")] Admin admin)
        {
            try
            {
                if (id != admin.AdminID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(admin);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AdminExists(admin.AdminID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(admin);
            }
            catch (Exception)
            {

                throw;
            }
        }



        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var admin = await _context.Admin
                    .FirstOrDefaultAsync(m => m.AdminID == id);
                if (admin == null)
                {
                    return NotFound();
                }

                return View(admin);
            }
            catch (Exception)
            {

                throw;
            }
        }



        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var admin = await _context.Admin.FindAsync(id);
                _context.Admin.Remove(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }
        }



        //Check whether Admin Exists or not.
        private bool AdminExists(int id)
        {
            try
            {
                return _context.Admin.Any(e => e.AdminID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
