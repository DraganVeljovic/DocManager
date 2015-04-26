using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Domain
{
    public class DocumentContent
    {
        public virtual int Id { get; set; }
        public virtual byte[] Data { get; set; }
    }
}
