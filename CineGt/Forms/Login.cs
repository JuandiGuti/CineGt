using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CineGt.Forms
{
    public partial class Login : Form
    {
        DBCineGt db;
        public Login()
        {
            InitializeComponent();
            db = new DBCineGt();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home();
            homeForm.Show();

            this.Hide();

            homeForm.FormClosed += (s, args) => Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String username = this.textBox1.Text;
            String password = this.textBox2.Text;
            try
            {
                var (isLoggedIn, userRole) = db.userLogIn(username, password);

                if (!isLoggedIn)
                {
                    throw new Exception("The username do not exists or the password is incorrect.");
                }
                else
                {
                    if (userRole == 1)
                    {
                        AdminView adminForm = new AdminView(username);
                        adminForm.Show();

                        this.Hide();

                        adminForm.FormClosed += (s, args) => Application.Exit();
                    }
                    else
                    {
                        EmployeeView adminForm = new EmployeeView(username);
                        adminForm.Show();

                        this.Hide();

                        adminForm.FormClosed += (s, args) => Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
