using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CineGt.Forms
{
    public partial class Home : Form
    {
        DBCineGt db;
        public Home()
        {
            InitializeComponent();
            db = new DBCineGt();
            if (db.Ok())
            {
                MessageBox.Show("Conectao");
            }
            else
            {
                MessageBox.Show("Nelson");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();

            this.Hide();

            loginForm.FormClosed += (s, args) => Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.Show();

            this.Hide();

            registerForm.FormClosed += (s, args) => Application.Exit();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }
    }
}
