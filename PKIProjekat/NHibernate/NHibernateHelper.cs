using NHibernate;
using NHibernate.Cfg;
using PKIProjekat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace PKIProjekat.NHibernate
{
    class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Fluently.Configure()
                        .Database(MsSqlCeConfiguration.Standard.
                            ConnectionString("Data Source=C:\\Users\\Dragan\\Documents\\Visual Studio 2012\\Projects\\PKIProjekat\\PKIProjekat\\PKIDB.sdf"))
                        .Mappings(m =>
                            {
                                m.FluentMappings.AddFromAssemblyOf<Employee>();
                                m.FluentMappings.AddFromAssemblyOf<Document>();
                             }).BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
