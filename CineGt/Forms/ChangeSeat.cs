using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace CineGt.Forms
{
    public partial class ChangeSeat : Form
    {
        DBCineGt db;
        public ChangeSeat()
        {
            InitializeComponent();
            db = new DBCineGt();
            dataGridView1.DataSource = null;
        }

        private void ChangeSeat_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                var selectedValue = selectedRow.Cells[4].Value.ToString();
                label2.Text = "Selected Seat:  " + selectedValue;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userEmail = textBox1.Text;
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = db.llenarDGchangeSeat(userEmail);
                dataGridView1.Columns[0].Width = 130;
                dataGridView1.Columns[1].Width = 130;
                dataGridView1.Columns[2].Width = 135;
                dataGridView1.Columns[2].HeaderText = "Buy Date";
                dataGridView1.Columns[3].Width = 135;
                dataGridView1.Columns[3].HeaderText = "ModdSeat Date";
                dataGridView1.Columns[4].Width = 50;
                dataGridView1.Columns[5].Width = 150;
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int idTransaction = int.Parse(selectedRow.Cells[0].Value.ToString());
                int idSession = int.Parse(selectedRow.Cells[1].Value.ToString());
                string actualSeat = selectedRow.Cells[4].Value.ToString();

                deploySeats deploySeats = new deploySeats(idTransaction, idSession, actualSeat);
                deploySeats.ShowDialog();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Select a seat.", "Selection Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
