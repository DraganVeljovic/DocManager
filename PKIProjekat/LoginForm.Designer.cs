namespace PKIProjekat
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loginControl1 = new PKIProjekat.LoginControl();
            this.SuspendLayout();
            // 
            // loginControl1
            // 
            this.loginControl1.Location = new System.Drawing.Point(243, 71);
            this.loginControl1.Name = "loginControl1";
            this.loginControl1.Password = "";
            this.loginControl1.Size = new System.Drawing.Size(306, 170);
            this.loginControl1.TabIndex = 7;
            this.loginControl1.Username = "";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.Controls.Add(this.loginControl1);
            this.Name = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Controls.SetChildIndex(this.loginControl1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LoginControl loginControl1;
    }
}
