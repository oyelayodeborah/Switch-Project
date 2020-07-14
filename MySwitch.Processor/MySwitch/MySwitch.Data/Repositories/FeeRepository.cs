using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySwitch.Core.Models;

namespace MySwitch.Data.Repositories
{
    public class FeeRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Fee> Repo = new BaseRepository<Fee>(new ApplicationDbContext());

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

        public Fee Get(int? id)
        {
            return Repo.Get(id);
        }
        public IEnumerable<Fee> GetByName(string Name)
        {
            return _context.Fees.Where(c => c.Name == Name);
        }
        public IEnumerable<Fee> GetAll()
        {
            return _context.Fees.ToList();
        }
        public void Update(Fee channel)
        {
            Repo.Update(channel);
        }

        public void Save(Fee channel)
        {
            Repo.Save(channel);
        }
    }
}
