using FluentNHibernate.Mapping;
using PKIProjekat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Mapping
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Id(x => x.Id);
            Map(x => x.Text);
            Map(x => x.Created);
            References(x => x.Document)
                .Not.LazyLoad();
            References(x => x.Owner)
                .Not.LazyLoad();
        }
    }
}
