using System.Text.RegularExpressions;
using BCrypt.Net;

namespace CineGt.Forms
{
    public partial class Register : Form
    {
        DBCineGt db;
        public Register()
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
            //REGISTER
            String username = this.textBox1.Text.ToLower();
            String password = this.textBox2.Text;
            String rePassword = this.textBox3.Text;
            try
            {
                if (username == "" || password == "" || rePassword == "")
                {
                    throw new Exception("Fill all the blanks to continue.");
                }
                else if (password != rePassword)
                {
                    throw new Exception("Passwords do not match.");
                }
                else if (!passwordSecure(password))
                {
                    throw new Exception("The password is not secure. Min. 8 characters, 1 Capital letter, 1 Special characters");
                }
                else
                {
                    //HASHEAR LA PASSWORD
                    password = hashPassword(password);

                    //BD - SP - NEWUSER
                    db.userRegister(username, password);

                    MessageBox.Show($"The user: {username}, is registered. Login to continue!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        private bool passwordSecure(string password)
        {
            // Al menos 8 caracteres, una letra mayúscula, una minúscula, un número y un carácter especial
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(password, passwordPattern);
        }
        private string hashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 5);
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
    }
}
