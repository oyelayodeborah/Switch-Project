using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MySwitch.Core.Models;
using MySwitch.Data.Repositories;
namespace MySwitch.Controllers
{
    public class SchemeController:Controller
    {
        SchemeRepository Repo = new SchemeRepository();

        public ActionResult Add()
        {
            var _context = new ApplicationDbContext();
            Scheme model = new Scheme();
            model.Routes = _context.Routes.ToList();
            if (model.Routes.Count() == 0)
            {
                model.Routes =  new List<Route>();;
            }
            model.Combos = _context.Combos.ToList();
            if (model.Combos.Count() == 0)
            {
                model.Combos = new List<Combo>(); ;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Scheme model)
        {
            var _context = new ApplicationDbContext();

            model.Routes = _context.Routes.ToList();
            if (model.Routes.Count() == 0)
            {
                model.Routes = new List<Route>(); ;
            }
            model.Combos = _context.Combos.ToList();
            if (model.Combos.Count() == 0)
            {
                model.Combos = new List<Combo>(); ;
            }
            try
            {
                if (!(Repo.isUniqueName(model.Name) && Repo.isUniqueCombo(model.ComboId)))
                {
                    ViewBag.Message = "Scheme's name and combo must be unique";
                    return View();
                }
                //check uniqueness of name and code
                
                //else if (!(Repo.isUniqueCombo(model.ComboId)))
                //{

                //    ViewBag.Message = "Scheme's Combo must be unique";
                //    return View(model);
                //}
                //else if (!(Repo.isUniqueRoute(model.RouteId)))
                //{
                //    ViewBag.Message = "Scheme's Route  must be unique";
                //    return View(model);

                //}

                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                Repo.Save(model);
                return RedirectToAction("Index", new { message = "Successfully added Scheme!" });
            }
            catch (Exception ex)
            {
                return View(new { message = "Scheme was not successful" });
            }
        }
        public ActionResult Edit(int? id)
        {
            var _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }

            Scheme model = _context.Schemes.Find(id);
            model.Routes = _context.Routes.ToList();
            if (model.Routes.Count() == 0)
            {
                model.Routes =  new List<Route>();;
            }
            model.Combos = _context.Combos.ToList();
            if (model.Combos.Count() == 0)
            {
                model.Combos = new List<Combo>(); ;
            }
            if (model ==  null)
            {
                return HttpNotFound();
            }

            return View(model);


        }

        [HttpPost]
        public ActionResult Edit(Scheme model)
        {
            var _context = new ApplicationDbContext();
            model.DateModified = DateTime.Now;

            try
            {

                var newModel = Repo.Get(model.Id);

                //check uniqueness of name and code
                //if (!(Repo.isUniqueCombo(newModel.ComboId, model.ComboId)))
                //{
                //    if (!(Repo.isUniqueRoute(newModel.RouteId, model.RouteId)))
                //    {
                //        ViewBag.Message = "Scheme's route and combo must be unique";
                //        return View();
                //    }
                //}

                if (!(Repo.isUniqueName(model.Name) && Repo.isUniqueCombo(model.ComboId)))
                {
                    ViewBag.Message = "Scheme's name and combo must be unique";
                    return View();
                }

                newModel.RouteId = model.RouteId;
                newModel.ComboId = model.ComboId;
                newModel.Description = model.Description;
                newModel.DateModified = model.DateModified;
                Repo.Update(newModel);
                ViewBag.Message = "Updated";
                return RedirectToAction("Index", new { message = "Scheme was successfully updated" });
            }
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Error updating Scheme" });
            }
        }

        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList.Count() == 0 ? new List<Scheme>() : getList);
        }
    }
}


