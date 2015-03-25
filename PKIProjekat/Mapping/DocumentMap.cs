using FluentNHibernate.Mapping;
using PKIProjekat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Mapping
{
    public class DocumentMap : ClassMap<Document>
    {
        public DocumentMap()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.KeyWords);
            Map(x => x.Type);
            References(x => x.Owner)
                .Not.LazyLoad();
            Map(x => x.Created);
            Map(x => x.Version);
            Map(x => x.IsReading);
            Map(x => x.IsWriting);
            Map(x => x.IsActive);

            HasManyToMany(x => x.Readers)
                .Cascade.All()
                .Table("DocumentReader")
                .Not.LazyLoad();

            HasManyToMany(x => x.Writers)
                .Cascade.All()
                .Table("DocumentWriter")
                .Not.LazyLoad();

        }
    }
}
