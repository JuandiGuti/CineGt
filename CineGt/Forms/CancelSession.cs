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
    public partial class CancelSession : Form
    {
        public String Username;
        DBCineGt db;
        public CancelSession(String username)
        {
            InitializeComponent();
            Username = username;
            db = new DBCineGt();
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = db.llenarDGmovieSession();
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 60;
            dataGridView1.Columns[4].Width = 243;
            dataGridView1.Refresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void CancelSession_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = int.Parse(selectedRow.Cells[0].Value.ToString());

                try
                {
                    db.cancelSession(id);
                    MessageBox.Show($"The session: {id} was canceled.");
                    dataGridView1.DataSource = null;
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = db.llenarDGmovieSession();
                    dataGridView1.Columns[0].Width = 30;
                    dataGridView1.Columns[1].Width = 200;
                    dataGridView1.Columns[2].Width = 200;
                    dataGridView1.Columns[3].Width = 60;
                    dataGridView1.Columns[4].Width = 243;
                    dataGridView1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Session Canceled Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Select a session to deactivate.");
            }
        }
    }
}
