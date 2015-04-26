using FluentNHibernate.Mapping;
using PKIProjekat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Mapping
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id);
            Map(x => x.Username);
            Map(x => x.Password);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Email);
            Map(x => x.Telephone);
            Map(x => x.OfficeNumber);
            Map(x => x.Administrator);

            HasMany(x => x.Documents)
                //.Inverse()
                .KeyColumn("owner_id")
                .Not.LazyLoad()
                .Cascade.All();

            HasManyToMany(x => x.Reading)
                .Inverse()
                //.Cascade.All()
                .Table("DocumentReader")
                .Not.LazyLoad();

            HasManyToMany(x => x.Writing)
                .Inverse()
                //.Cascade.All()
                .Table("DocumentWriter")
                .Not.LazyLoad();
        }
        
    }
}
