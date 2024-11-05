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
    public partial class buyTicketsTransaction : Form
    {
        DBCineGt db;
        string Username;
        public buyTicketsTransaction(string username)
        {
            InitializeComponent();
            Username = username;
            db = new DBCineGt();
            db.llenarComboBox(comboBox1, "MovieName", "Movie");
        }

        private void buyTicketsTransaction_Load(object sender, EventArgs e)
        {

        }
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                try
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = db.llenarDGsessionByMovie(comboBox1.Text);
                    dataGridView1.Columns[0].Width = 30;
                    dataGridView1.Columns[1].Width = 150;
                    dataGridView1.Columns[2].Width = 150;
                    dataGridView1.Columns[3].Width = 60;
                    dataGridView1.Columns[4].Width = 230;
                    dataGridView1.Columns[5].HeaderText = "Available Seats";
                    dataGridView1.Columns[5].Width = 100;
                    dataGridView1.Refresh();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

            }
            else
            {
                MessageBox.Show("Select a movie.", "Session Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            NewClient form = new NewClient();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" | comboBox2.Text ==""| textBox1.Text =="")
            {
                MessageBox.Show("Fill all the blanks.", "Error. New Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string email = textBox1.Text;
            int numberSeats = int.Parse(comboBox2.Text);
            int idMovieSession = 0;
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Select a movie session.", "Select a session", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            idMovieSession = int.Parse(selectedRow.Cells[0].Value.ToString());
            int room = int.Parse(selectedRow.Cells[3].Value.ToString());
            try
            {
                int idTransaction = db.newTicketTransaction(email, Username, numberSeats, idMovieSession);
                
                buySeats buySeats = new buySeats(idTransaction, idMovieSession, numberSeats, Username, room);
                buySeats.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
