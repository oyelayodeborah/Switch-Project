using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySwitch.Core.Models;
using MySwitch.Data.Repositories;

namespace MySwitch.Controllers
{
    public class RouteController:Controller
    {
        RouteRepository Repo = new RouteRepository();

        public ActionResult Add()
        {
            var _context = new ApplicationDbContext();
            Route model = new Route();
            model.SinkNodes = _context.SinkNodes.ToList();
            if (model.SinkNodes.Count() == 0)
            {
                model.SinkNodes =  new List<SinkNode>();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Route model)
        {
            var _context = new ApplicationDbContext();
            try
            {
                //check uniqueness of name and code
                if (!(Repo.isUniqueCardPan(model.CardPAN)))
                {
                    ViewBag.Message = "Route's cardPAN must be unique";
                    if (!(Repo.isUniqueName(model.Name)))
                    {
                        ViewBag.Message = "Route's name already exist";

                    }
                    return View();
                }

                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                Repo.Save(model);
                return RedirectToAction("Index", new { message = "Successfully added Route!" });
            }
            catch (Exception ex)
            {
                return View(new { message = "Route was not successful" });
            }
    }
        public ActionResult Edit(int? id)
        {
            var _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }
            
            Route model = _context.Routes.Find(id);
            model.SinkNodes = _context.SinkNodes.ToList();
            if (model.SinkNodes.Count() == 0)
            {
                model.SinkNodes = new List<SinkNode>();

            }

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);



        }

        [HttpPost]
        public ActionResult Edit(Route model)
        {
            var _context = new ApplicationDbContext();
            model.DateModified = DateTime.Now;

            try
            {

                var newModel = Repo.Get(model.Id);

                //check uniqueness of name and code
                if (!(Repo.isUniqueCardPan(newModel.CardPAN, model.CardPAN)))
                {
                    ViewBag.Message = "Route's cardPan must be unique";
                    if (!(Repo.isUniqueName(newModel.Name, model.Name)))
                    {
                        ViewBag.Message = "Route's name must be unique";
                        return View();
                    }
                    return View();
                }
                newModel.Name = model.Name;
                newModel.CardPAN = model.CardPAN;
                newModel.Description = model.Description;
                newModel.SinkNodeId = model.SinkNodeId;
                newModel.DateModified = model.DateModified;
                Repo.Update(newModel);
                ViewBag.Message = "Updated";
                return RedirectToAction("Index", new { message = "Route was successfully updated" });
            }
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Error updating Route" });
            }
        }


        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList == null ? new List<Route>() : getList);
        }
    }
}

