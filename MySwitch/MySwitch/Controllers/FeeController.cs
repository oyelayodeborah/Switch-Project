using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySwitch.Core.Models;
using MySwitch.Data.Repositories;

namespace MySwitch.Controllers
{
    public class FeeController:Controller
    {
        FeeRepository Repo = new FeeRepository();

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Fee model)
        {
            try
            {
                //check uniqueness of name and code
                if (!(Repo.isUniqueName(model.Name)))
                {
                    
                   ViewBag.Message = "Fee's name must be unique";

                    return View();
                }

                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                Repo.Save(model);
                return RedirectToAction("Index", new { message = "Successfully added Fee!" });
            }
            catch (Exception ex)
            {
                return View(new { message = "Fee was not successfully added" });
            }
        }
        public ActionResult Edit(int? id)
        {
            var _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }

            Fee channels = _context.Fees.Find(id);
            if (channels == null)
            {
                return HttpNotFound();
            }

            return View(channels);


        }

        [HttpPost]
        public ActionResult Edit(Fee model)
        {
            var _context = new ApplicationDbContext();
            model.DateModified = DateTime.Now;

            try
            {

                var newModel = Repo.Get(model.Id);

                //check uniqueness of name and code
                if (!(Repo.isUniqueName(newModel.Name, model.Name)))
                {
                        ViewBag.Message = "Fee's name must be unique";
                        return View();
                }
                newModel.Name = model.Name;
                newModel.Maximum = model.Maximum;
                newModel.Minimum = model.Minimum;
                newModel.PercentOfTransaction = model.PercentOfTransaction;
                newModel.FlatAmount = model.FlatAmount;
                newModel.DateModified = model.DateModified;
                Repo.Update(newModel);
                ViewBag.Message = "Updated";
                return RedirectToAction("Index", new { message = "Fee was successfully updated" });
            }
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Error updating Fee" });
            }
        }

        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList == null ? new List<Fee>() : getList);
        }
    }
}
   