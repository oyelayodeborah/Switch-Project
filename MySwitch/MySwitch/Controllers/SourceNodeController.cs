using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySwitch.Core.Models;
using MySwitch.Data.Repositories;

namespace MySwitch.Controllers
{
    public class SourceNodeController:Controller
    {
        SourceNodeRepository Repo = new SourceNodeRepository();


        public ActionResult Add()
        {
            var _context = new ApplicationDbContext();
            SourceNode model = new SourceNode();
            model.Schemes = _context.Schemes.ToList();
            if (model.Schemes.Count()==0)
            {
                model.Schemes = new List<Scheme>();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(SourceNode model)
        {
            var _context = new ApplicationDbContext();
            try
            {
                //check uniqueness of name and code
                if (!(Repo.isUniqueName(model.Name)))
                {
                    ViewBag.Message = "Source Node's name must be unique";
                    return View(model);
                }
                model.HostName = model.IPAddress;
                model.Status = Status.Active;
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                Repo.Save(model);
                return RedirectToAction("Index", new { message = "Successfully added Source Node!" });
            }
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Source Node was not successful" });
            }
        }
        public ActionResult Edit(int? id)
        {
            var _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }

            SourceNode model = _context.SourceNodes.Find(id);
            model.Schemes = _context.Schemes.ToList();
            if (model.Schemes.Count() == 0)
            {
                model.Schemes = new List<Scheme>();
            }
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);


        }

        [HttpPost]
        public ActionResult Edit(SourceNode model)
        {
            var _context = new ApplicationDbContext();
            try
            {
                //if (ModelState.IsValid)
                //{
                var node = Repo.Get(model.Id);

                //check uniqueness of name and code
                if (!(Repo.isUniqueName(node.Name, model.Name)))
                {
                    ViewBag.Message = "Source Node's name must be unique";
                    return View();
                }
                node.HostName = model.IPAddress;
                node.IPAddress = model.IPAddress;
                node.Name = model.Name;
                node.Port = model.Port;
                node.DateModified = DateTime.Now;
                Repo.Update(node);
                ViewBag.Message = "Updated";
                return RedirectToAction("Index", new { message = "Source Node was successfully updated" });
            }
            //    ViewBag.Message = "Please enter correct data";
            //    return View();
            //}
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Error updating node" });
            }
        }

        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList==null? new List<SourceNode>():getList);
        }
    }
}