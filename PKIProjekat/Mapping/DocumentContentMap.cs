using FluentNHibernate.Mapping;
using NHibernate.Type;
using PKIProjekat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Mapping
{
    public class DocumentContentMap : ClassMap<DocumentContent>
    {
        public DocumentContentMap()
        {
            Id(x => x.Id);
            Map(x => x.Data).CustomType<BinaryBlobType>();
        }
    }
}
