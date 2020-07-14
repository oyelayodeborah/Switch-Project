using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySwitch.Core.Models;

namespace MySwitch.Data.Repositories
{
    public class SchemeRepository
    {
        public BaseRepository<Scheme> Repo = new BaseRepository<Scheme>(new ApplicationDbContext());

        public bool isUniqueName(string name)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(name.ToLower())))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueName(string oldName, string newName)
        {
            bool flag = true;
            if (!oldName.ToLower().Equals(newName.ToLower()))
            {
                if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(newName.ToLower())))
                {
                    flag = false;
                }
            }
            return flag;
        }
        public bool isUniqueRoute(int route)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.RouteId.Equals(route)))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueRoute(int oldRoute, int newRoute)
        {
            bool flag = true;
            if (!oldRoute.Equals(newRoute))
            {
                if (Repo.GetAll().Any(n => n.RouteId.Equals(newRoute)))
                {
                    flag = false;
                }
            }
            return flag;
        }

        public bool isUniqueCombo(int combo)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.ComboId.Equals(combo)))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueCombo(int oldCombo, int newCombo)
        {
            bool flag = true;
            if (!oldCombo.Equals(newCombo))
            {
                if (Repo.GetAll().Any(n => n.ComboId.Equals(newCombo)))
                {
                    flag = false;
                }
            }
            return flag;
        }

        public Scheme Get(int? id)
        {
            return Repo.Get(id);
        }
        public IEnumerable<Scheme> GetByRouteId(int RouteId)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            return _context.Schemes.Where(c => c.RouteId == RouteId);
        }
        public IEnumerable<Scheme> GetAll()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var getList = _context.Schemes.ToList();
            //var getList = Repo.GetAll();
            return getList;
        }
        public void Update(Scheme channel)
        {
            Repo.Update(channel);
        }

        public void Save(Scheme channel)
        {
            Repo.Save(channel);
        }
    }
}
