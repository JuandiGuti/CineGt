using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CineGt.Forms
{
    public partial class NewSession : Form
    {
        public String Username;
        DBCineGt db;
        public NewSession(String username)
        {
            InitializeComponent();
            db = new DBCineGt();
            Username = username;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy                      hh:mm tt";

            db.llenarComboBox(comboBox2, "MovieName", "Movie");
            db.llenarComboBox(comboBox1, "ID", "Room");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void NewSession_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime beginningDate = DateTime.Parse(dateTimePicker1.Text);
            string movieName = comboBox2.Text;
            int room = int.Parse(comboBox1.Text);

            try
            {
                db.newSession(beginningDate, movieName, room, Username);
                MessageBox.Show("Session created!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "New Session Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
