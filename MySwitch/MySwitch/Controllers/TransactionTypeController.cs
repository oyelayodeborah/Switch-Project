using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySwitch.Core.Models;
using MySwitch.Data.Repositories;

namespace MySwitch.Controllers
{
    public class TransactionTypeController:Controller
    {
        TransactionTypeRepository Repo = new TransactionTypeRepository();

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(TransactionType model)
        {
            
            try
            {
                //check uniqueness of name and code
                if (!(Repo.isUniqueCode(model.Code)))
                {
                    ViewBag.Message = "Combo's name must be unique";
                    if (!(Repo.isUniqueName(model.Name)))
                    {
                        ViewBag.Message = "Combo already exist";
                        
                    }
                    return View();
                }
                
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                Repo.Save(model);
                return RedirectToAction("Index", new { message = "Successfully added Combo!" });
            }
            catch (Exception ex)
            {
                return View(new { message = "Combo was not successful" });
            }
        }
        public ActionResult Edit(int? id)
        {
            var _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }

            TransactionType model = _context.TransactionTypes.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);


        }

        [HttpPost]
        public ActionResult Edit(TransactionType model)
        {
            var _context = new ApplicationDbContext();
            model.DateModified = DateTime.Now;

            try
            {

                var newModel = Repo.Get(model.Id);

                //check uniqueness of name and code
                if (!(Repo.isUniqueCode(newModel.Code, model.Code)))
                {
                    ViewBag.Message = "Combo's name must be unique";
                    if (!(Repo.isUniqueName(newModel.Name, model.Name)))
                    {
                        ViewBag.Message = "Combo's name must be unique";
                        return View();
                    }
                    return View();
                }
                newModel.Name = model.Name;
                newModel.Code = model.Code;
                newModel.Description = model.Description;
                newModel.DateModified = model.DateModified;
                Repo.Update(newModel);
                ViewBag.Message = "Updated";
                return RedirectToAction("Index", new { message = "Combo was successfully updated" });
            }
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Error updating Combo" });
            }
        }
    

        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList == null ? new List<TransactionType>() : getList);
        }
    }
}