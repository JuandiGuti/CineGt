using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;

namespace CineGt.Forms
{
    public partial class NewClient : Form
    {
        DBCineGt db;
        public NewClient()
        {
            InitializeComponent();
            db = new DBCineGt();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string name = textBox1.Text;
            string email = textBox2.Text;
            string phone = textBox3.Text;
            int age = int.Parse(comboBox1.Text);

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address ex. pepe@gmail.com");
                return;
            }

            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Please enter a valid phone number in the format ex. +50265328954.");
                return;
            }

                db.newClient(name, email, phone, age);
                MessageBox.Show("Client data saved successfully!");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            string phonePattern = @"^\+\d{11,14}$"; // Acepta un + seguido de entre 11 y 14 dígitos
            return Regex.IsMatch(phoneNumber, phonePattern);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
