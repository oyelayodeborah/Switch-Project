using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySwitch.Core.Models;

namespace MySwitch.Data.Repositories
{
    public class ChannelRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Channel> Repo = new BaseRepository<Channel>(new ApplicationDbContext());

        public Channel GetByCode(string code)
        {
            var transtype = _context.Channels.Where(c => c.Code == code).FirstOrDefault();

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
        public Channel Get(int? id)
        {
            return Repo.Get(id);
        }
        public IEnumerable<Channel> GetByName(string Name)
        {
            return _context.Channels.Where(c => c.Name == Name);
        }
        public IEnumerable<Channel> GetAll()
        {
            return _context.Channels.ToList();
        }
        public void Update(Channel channel)
        {
            Repo.Update(channel);
        }

        public void Save(Channel channel)
        {
            Repo.Save(channel);
        }
    }
}
