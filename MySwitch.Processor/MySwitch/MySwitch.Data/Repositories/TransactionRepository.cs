using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySwitch.Core.Models;

namespace MySwitch.Data.Repositories
{
    public class TransactionRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Transaction> Repo = new BaseRepository<Transaction>(new ApplicationDbContext());

        public Transaction Get(int? id)
        {
            return Repo.Get(id);
        }
        public Transaction GetByOriginalDataElement(string OriginalDataElement)
        {
            return _context.Transactions.Where(c => c.OriginalDataElement == OriginalDataElement).SingleOrDefault();
        }
        public IEnumerable<Transaction> GetByCardPAN(string CardPAN)
        {
            return _context.Transactions.Where(c => c.CardPAN == CardPAN);
        }
        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }
        public void Update(Transaction channel)
        {
            Repo.Update(channel);
        }

        public void Save(Transaction channel)
        {
            Repo.Save(channel);
        }
    }
}
