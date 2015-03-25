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
    public class CommentRepository
    {
        public void Add(Comment comment)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(comment);
                    transaction.Commit();
                }
            }
        }

        public IList<Comment> GetCommentsForDocument(Document document)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Comment>().Where(x => x.Document == document).List<Comment>();
                return result;// ?? new User();
            }
        }

        public void Delete(Comment comment)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(comment);
                    transaction.Commit();
                }
            }
        }
    }
}
