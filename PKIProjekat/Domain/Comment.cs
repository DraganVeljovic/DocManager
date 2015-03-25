using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Domain
{
    public class Comment
    {
        public virtual int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual Document Document { get; set; }
        public virtual Employee Owner { get; set; }
    }
}
