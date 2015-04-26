using PKIProjekat.Domain;
using PKIProjekat.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKIProjekat
{
    public partial class NewComment : Form
    {
        private Document selectedDocument;
        private Employee loggedEmployee;

        public NewComment(Document selectedDocument, Employee loggedEmployee)
        {
            InitializeComponent();

            this.selectedDocument = selectedDocument;
            this.loggedEmployee = loggedEmployee;

            label3.Text = selectedDocument.Title;
            label4.Text = selectedDocument.Version.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CommentRepository commentRepository = new CommentRepository();

            Comment newComment = new Comment();
            newComment.Text = textBox1.Text;
            newComment.Created = DateTime.Now;
            newComment.Document = selectedDocument;
            newComment.Owner = loggedEmployee;

            commentRepository.Add(newComment);

            Dispose();
        }
    }
}
