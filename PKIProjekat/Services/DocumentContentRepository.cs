using NHibernate;
using PKIProjekat.Domain;
using PKIProjekat.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Services
{
    public class DocumentContentRepository
    {
        public void Add(DocumentContent content)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(content);
                    transaction.Commit();
                }
            }
        }

        public DocumentContent GetContentById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<DocumentContent>().Where(x => x.Id == id).SingleOrDefault();
                return result;// ?? new User();
            }
        }

        public void Delete(DocumentContent content)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(content);
                    transaction.Commit();
                }
            }
        }
    }
}
