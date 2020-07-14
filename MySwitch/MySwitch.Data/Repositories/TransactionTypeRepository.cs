using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySwitch.Core.Models;

namespace MySwitch.Data.Repositories
{
    public class TransactionTypeRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<TransactionType> Repo = new BaseRepository<TransactionType>(new ApplicationDbContext());
        public TransactionType GetByCode(string code)
        {
            var transtype = _context.TransactionTypes.Where(c => c.Code == code).FirstOrDefault();
            
            return transtype;
        }
        public bool isUniqueCode(string code)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.Code.ToLower().Equals(code.ToLower())))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueCode(string oldCode, string newCode)
        {
            bool flag = true;
            if (!newCode.ToLower().Equals(newCode.ToLower()))
            {
                if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(newCode.ToLower())))
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
        public TransactionType Get(int? id)
        {
            return Repo.Get(id);
        }
        public IEnumerable<TransactionType> GetByName(string Name)
        {
            return _context.TransactionTypes.Where(c => c.Name == Name);
        }
        public IEnumerable<TransactionType> GetAll()
        {
            return _context.TransactionTypes.ToList();
        }
        public void Update(TransactionType channel)
        {
            Repo.Update(channel);
        }

        public void Save(TransactionType channel)
        {
            Repo.Save(channel);
        }
    }
}
