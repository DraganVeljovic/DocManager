using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKIProjekat.Domain
{
    public class Document
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string KeyWords { get; set; }
        public virtual string Type { get; set; }
        public virtual Employee Owner { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual int Version { get; set; }
        public virtual int IsReading { get; set; }
        public virtual bool IsWriting { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DocumentContent Content { get; set; }

        public virtual IList<Employee> Readers { get; set; }
        public virtual IList<Employee> Writers { get; set; }

        public Document()
        {
            Readers = new List<Employee>();
            Writers = new List<Employee>();

            Created = DateTime.Now;
            Version = 0;
            IsReading = 0;
            IsWriting = false;
            IsActive = true;
        }

        public Document(Document document) : this()
        {
            this.Title = document.Title;
            this.KeyWords = document.KeyWords;
            this.Type = document.Type;
            this.Created = document.Created;
            this.Version = document.Version;
            this.Content = document.Content;

            foreach (var emp in document.Readers)
                this.Readers.Add(emp);

            foreach (var emp in document.Writers)
                this.Writers.Add(emp);
        }

    }
}
