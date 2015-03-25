using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Domain
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual int Telephone { get; set; }
        //public virtual string Profession { get; set; }
        public virtual int OfficeNumber { get; set; }
        public virtual bool Administrator { get; set;  }

        public virtual IList<Document> Documents { get; set; }

        public virtual IList<Document> Reading { get; set; }
        public virtual IList<Document> Writing { get; set; }

        public Employee()
        {
            Documents = new List<Document>();
            Reading = new List<Document>();
            Writing = new List<Document>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Employee) 
            {
                return ((Employee)obj).Username.CompareTo(Username) == 0 ? true : false;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

    }
}
