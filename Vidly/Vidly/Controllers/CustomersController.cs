using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers/Index
        public ActionResult Index()
        {
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();

            return View(customers);
        }

        // GET: Customers/Details/Id
        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).FirstOrDefault(c => c.Id == id);
            
            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }                

        // GET: Customers/New
        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();

            var viewModel = new CustomerFormViewModel()
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes,
                Title = "New Customer"
            };

            return View("CustomerForm",viewModel);
        }

        // POST: Customers/Save
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel()
                {
                    Title = customer.Id == 0 ? "New Customer" : "Edit Customer",
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };

                return View("CustomerForm", viewModel);
            }

            if(customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customers.First(c => c.Id == customer.Id);

                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
            }
                        
            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }

        // GET: Customers/Edit/Id
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            
            if (customer == null)
                return HttpNotFound();

            var viewModel = new CustomerFormViewModel()
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList(),
                Title = "Edit Customer"
            };

            return View("CustomerForm", viewModel);
        }

        // POST: Customers/Delete/Id
        [HttpPost]
        public ActionResult Delete(int[] customer_id)
        {
            if (customer_id != null && customer_id.Length > 0)
            {
                var selectedCustomers = _context.Customers.Where(c => customer_id.Contains(c.Id)).ToList();
                _context.Customers.RemoveRange(selectedCustomers);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Customers");
        }

    }
}