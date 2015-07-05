using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PKIProjekat.Domain;
using PKIProjekat.NHibernate;
using NHibernate;
using NHibernate.Criterion;

namespace PKIProjekat.Services
{
    public class DocumentRepository
    {
        public void Add(Document document)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(document);
                    transaction.Commit();
                }
            }
        }

        public IList<Document> GetAllDocuments()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Document>().List<Document>();
            }
        }

        public IList<Document> GetDocumentsByTitle(string title)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Document>().Where(x => x.Title == title).List<Document>();
                return result;// ?? new User();
            }
        }

        public Document GetDocumentByVersion(string title, int version)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Document>().Where(
                    Restrictions.Eq("Title", title) && Restrictions.Eq("Version", version))
                    .SingleOrDefault();
                return result;// ?? new User();
            }
        }

        public IList<Document> GetAllDocumentsForEmployee(Employee employee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var ownDocuments = session.QueryOver<Document>()
                    .Where(x => x.Owner == employee)
                    .List<Document>();

                foreach (Document doc in session.QueryOver<Document>()
                    .JoinQueryOver<Employee>(x => x.Readers)
                    .Where(y => y.Username == employee.Username)
                    .List<Document>())
                {
                    ownDocuments.Add(doc);
                }

                foreach (Document doc in session.QueryOver<Document>()
                    .JoinQueryOver<Employee>(x => x.Writers)
                    .Where(y => y.Username == employee.Username)
                    .List<Document>())
                {
                    ownDocuments.Add(doc);
                }

                return ownDocuments;
            }
        }

        public IList<Document> GetOwnDocumentsForEmployee(Employee employee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Document>()
                    .Where(x => x.Owner == employee)
                    .List<Document>();
            }
        }

        public IList<Document> GetReadableDocumentsForEmployee(Employee employee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Document>()
                    .JoinQueryOver<Employee>(x => x.Readers)
                    .Where(y => y.Username == employee.Username)
                    .List<Document>();
            }
        }

        public IList<Document> GetWritableDocumentsForEmployee(Employee employee)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {

                return session.QueryOver<Document>()
                    .JoinQueryOver<Employee>(x => x.Writers)
                    .Where(y => y.Username == employee.Username)
                    .List<Document>();
            }
        }

        public IList<Document> GetDocumentBySearchCriteria(string title, string keyWords, string type, 
            DateTime startDate, DateTime endDate, bool archived, bool active)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Document>()
                    .Where(
                        Restrictions.On<Document>(x => x.Title).IsLike("%" + title + "%") &&
                        Restrictions.On<Document>(x => x.KeyWords).IsLike("%" + keyWords + "%") &&
                        Restrictions.Eq("Type", type) &&
                        Restrictions.Gt("Created", startDate) &&
                        Restrictions.Lt("Created", endDate) &&
                        (Restrictions.Eq("IsActive", !archived) ||
                        Restrictions.Eq("IsActive", active))
                        )
                    .List<Document>();
            }
        }

        public void Update(Document document)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(document);
                    transaction.Commit();
                }
            }
        }

        public void Delete(Document document)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(document);
                    transaction.Commit();
                }
            }
        }

    }
}
