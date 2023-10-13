using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        public Guid Guid { get; }

        public Enterprise(Guid guid)
        {
            this.Guid = guid;
        }

        public string Name { get; set; }

        public void SetName(string name) { this.Name = name; }

        public string Inn {
            get
            {
                return this.Inn;
            }
            set
            {
                if (value.Length != 10 || !value.All(z => char.IsDigit(z)))
                    throw new ArgumentException();
                this.Inn = value;
            }
        }

        public DateTime EstablishDate { get; set; }

        public TimeSpan ActiveTimeSpan
        {
            get
            {
                return DateTime.Now - EstablishDate;
            }
        }
        
        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            var amount = 0.0;
            foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == Guid))
                amount += t.Amount;
            return amount;
        }
    }
}
