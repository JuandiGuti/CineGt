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
    public partial class NewMovie : Form
    {
        public String Username;
        DBCineGt db;
        public NewMovie(String username)
        {
            InitializeComponent();
            Username = username;
            db = new DBCineGt();
            db.llenarComboBox(comboBox1, "Clasification", "Clasification");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void NewMovie_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String movieName = textBox2.Text;
                String movieDescription = textBox1.Text;
                string strDuration = $"{comboBox4.Text}:{comboBox3.Text}:{comboBox2.Text}";
                String clasification = comboBox1.Text;

                if (movieName == "" | movieDescription == "" | comboBox4.Text == "" | comboBox3.Text == "" | comboBox2.Text == "" | comboBox1.Text == "")
                {
                    throw new Exception("Fill all the blanks.");
                }

                TimeSpan duration = TimeSpan.Parse(strDuration);

                db.newMovie(movieName, movieDescription, duration, clasification);

                MessageBox.Show("The movie is created.");
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
