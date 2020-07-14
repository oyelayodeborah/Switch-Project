using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySwitch.Core.Models;

namespace MySwitch.Data.Repositories
{
    public class ComboRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Combo> Repo = new BaseRepository<Combo>(new ApplicationDbContext());
        public bool isUniqueNameTrnTypeChannelFee(string name, int trntype,int channel, int fee)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(name.ToLower())) && Repo.GetAll().Any(n=>n.TransactionTypeId.Equals(trntype))
                && Repo.GetAll().Any(n => n.ChannelId.Equals(channel)) && Repo.GetAll().Any(n => n.FeeId.Equals(fee)))
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
        public bool isUniqueNameTrnTypeChannelFee(string oldName, string newName, int oldtrntype, int newtrntype, int oldchannel, int newchannel, int oldfee, int newfee)
        {
            bool flag = true;
            if (!oldName.ToLower().Equals(newName.ToLower()) && !oldtrntype.Equals(newtrntype) && !oldchannel.Equals(newchannel) && !oldfee.Equals(newfee))
            {
                if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(newName.ToLower())) && Repo.GetAll().Any(n => n.TransactionTypeId.Equals(newtrntype)) 
                    && Repo.GetAll().Any(n => n.ChannelId.Equals(newchannel)) && Repo.GetAll().Any(n => n.FeeId.Equals(newfee)))
                {
                    flag = false;
                }
            }
            return flag;
        }
        public bool isUniqueName(string name)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(name.ToLower())))
            {
                flag = false;
            }
            return flag;
        }

        public Combo Get(int? id)
        {
            return Repo.Get(id);
        }
        public IEnumerable<Combo> GetByName(string Name)
        {
            return _context.Combos.Where(c => c.Name == Name);
        }
        public IEnumerable<Combo> GetAll()
        {
            return _context.Combos.ToList();
        }
        public void Update(Combo model)
        {
            Repo.Update(model);
        }

        public void Save(Combo model)
        {
            Repo.Save(model);
        }
    }
}
