using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MySwitch.Core.Models;
using MySwitch.Data.Repositories;

namespace MySwitch.Controllers
{
    public class ChannelController : Controller
    {
        ChannelRepository Repo = new ChannelRepository();


        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Channel model)
        {
            try
            {
                //check uniqueness of name and code
                if (!(Repo.isUniqueCode(model.Code)))
                {
                    ViewBag.Message = "Channel's name must be unique";
                    if (!(Repo.isUniqueName(model.Name)))
                    {
                        ViewBag.Message = "Channel already exist";

                    }
                    return View();
                }

                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                Repo.Save(model);
                return RedirectToAction("Index", new { message = "Successfully added Channel!" });
            }
            catch (Exception ex)
            {
                return View(new { message = "Channel was not successful" });
            }
        }
        public ActionResult Edit(int? id)
        {
            var _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }

            Channel channels = _context.Channels.Find(id);
            if (channels == null)
            {
                return HttpNotFound();
            }

            return View(channels);


        }
        
        [HttpPost]
        public ActionResult Edit(Channel model)
        {
            var _context = new ApplicationDbContext();
            model.DateModified = DateTime.Now;

            try
            {

                var newModel = Repo.Get(model.Id);

                //check uniqueness of name and code
                if (!(Repo.isUniqueCode(newModel.Code, model.Code)))
                {
                    ViewBag.Message = "Channel's name must be unique";
                    if (!(Repo.isUniqueName(newModel.Name, model.Name)))
                    {
                        ViewBag.Message = "Channel's name must be unique";
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
                return RedirectToAction("Index", new { message = "Channel was successfully updated" });
            }
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Error updating Channel" });
            }
        }

        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList == null ? new List<Channel>() : getList);
        }
    }
}