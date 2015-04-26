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
    public partial class DisplayComment : Form
    {
        protected Comment comment;

        public DisplayComment(Comment comment, Employee loggedEmployee)
        {
            InitializeComponent();

            if (!(comment.Owner.Username.CompareTo(loggedEmployee.Username) == 0)
                && !(comment.Document.Owner.Username.CompareTo(loggedEmployee.Username) == 0)
                && !comment.Document.Writers.Contains(loggedEmployee)
                && !loggedEmployee.Administrator)
                button1.Enabled = false;

            this.comment = comment;

            label3.Text = comment.Owner.Username;

            textBox1.Text = comment.Text;
            textBox1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this comment?", "Confirmation dialog", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                (new CommentRepository()).Delete(comment);

                DialogResult = DialogResult.OK;
            }
        }
    }
}
