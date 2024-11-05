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
    public partial class cancelTransaction : Form
    {
        DBCineGt db;
        public cancelTransaction()
        {
            InitializeComponent();
            db = new DBCineGt();
            dataGridView1.DataSource = null;
        }

        private void cancelTransaction_Load(object sender, EventArgs e)
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
                var selectedValue = selectedRow.Cells[0].Value.ToString();
                label2.Text = "Selected Transaction:  " + selectedValue;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userEmail = textBox1.Text;
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = db.llenarDGcancelTransaction(userEmail);
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
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    int idTransaction = int.Parse(selectedRow.Cells[0].Value.ToString());

                    db.cancelTicketTransaction(idTransaction);

                    MessageBox.Show("The transaction was canceled.", "Transaction Canceled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Select a transaction.", "Selection Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
