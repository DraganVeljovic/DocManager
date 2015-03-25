using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PKIProjekat.Domain;
using PKIProjekat.NHibernate;
using NHibernate;

namespace PKIProjekat.Services
{
    public class EmployeeRepository
    {
        public void Add(Employee newEmployee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newEmployee);
                    transaction.Commit();
                }
            }
        }

        public Employee GetEmployeeByName(string username)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Employee>().Where(x => x.Username == username).SingleOrDefault();
                return result;// ?? new User();
            }
        }

        public IList<Employee> GetAllEmployees()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Employee>().List<Employee>();
                return result;// ?? new User();
            }
        }

        public void Update(Employee newEmployee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(newEmployee);
                    transaction.Commit();
                }
            }
        }

        public void Delete(Employee newEmployee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(newEmployee);
                    transaction.Commit();
                }
            }
        }

    }
}
