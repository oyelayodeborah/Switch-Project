using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MySwitch.ViewModels;
using MySwitch.Core.Models;
using MySwitch.Data.Repositories;

namespace MySwitch.Controllers
{
    public class ComboController : Controller
    {
        ComboRepository Repo = new ComboRepository();


        public ActionResult Add()
        {
            var _context = new ApplicationDbContext();
            Combo model = new Combo();
            model.Channels = _context.Channels.ToList();
            //if (model.Channels.Count() == 0)
            //{
            //    model.Channels = new List<Channel>(); ;
            //}
            model.Fees = _context.Fees.ToList();
            //if (model.Fees.Count() == 0)
            //{
            //    model.Fees = new List<Fee>(); ;
            //}
            model.TransactionTypes = _context.TransactionTypes.ToList();
            //if (model.TransactionTypes.Count() == 0)
            //{
            //    model.TransactionTypes = new List<TransactionType>(); ;
            //}
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Combo model)
        {
            try
            {
                //check uniqueness of name and code
                if (!(Repo.isUniqueName(model.Name)))
                {
                    ViewBag.Message = "Combo's name must be unique";
                    return View();
                }
                if (!(Repo.isUniqueNameTrnTypeChannelFee(model.Name, model.TransactionTypeId, model.ChannelId, model.FeeId)))
                {
                     ViewBag.Message = "Combo already exist";
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

            Combo model = _context.Combos.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            model.Channels = _context.Channels.ToList();
            //if (model.Channels.Count() == 0)
            //{
            //    model.Channels = new List<Channel>(); ;
            //}
            model.Fees = _context.Fees.ToList();
            //if (model.Fees.Count() == 0)
            //{
            //    model.Fees = new List<Fee>(); ;
            //}
            model.TransactionTypes = _context.TransactionTypes.ToList();
            //if (model.TransactionTypes.Count() == 0)
            //{
            //    model.TransactionTypes = new List<TransactionType>(); ;
            //}
            return View(model);


        }
        
        [HttpPost]
        public ActionResult Edit(Combo model)
        {
            var _context = new ApplicationDbContext();
            model.DateModified = DateTime.Now;

            try
            {
                
                var newModel = Repo.Get(model.Id);

                //check uniqueness of name and code
                if (!(Repo.isUniqueName(newModel.Name, model.Name)))
                {
                    ViewBag.Message = "Combo's name must be unique";
                    return View();
                }
                if (!(Repo.isUniqueNameTrnTypeChannelFee(newModel.Name, model.Name, newModel.TransactionTypeId, model.TransactionTypeId, newModel.ChannelId, model.ChannelId, newModel.FeeId, model.FeeId)))
                {
                    ViewBag.Message = "Combo already exist";
                    return View();
                }
                newModel.Name = model.Name;
                newModel.TransactionTypeId = model.TransactionTypeId;
                newModel.ChannelId = model.ChannelId;
                newModel.FeeId = model.FeeId;
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
            return View(getList == null ? new List<Combo>() : getList);
        }
    }
}