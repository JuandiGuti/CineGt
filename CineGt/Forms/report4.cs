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
    public partial class report4 : Form
    {
        DBCineGt db;
        public report4()
        {
            InitializeComponent();
            db = new DBCineGt();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void report4_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "")
                {
                    dataGridView1.DataSource = null;
                    decimal n = decimal.Parse(comboBox1.Text) / 100;
                    dataGridView1.DataSource = db.llenarDGGetLowOccupancySessions(n);
                    dataGridView1.Refresh();
                }
                else
                {
                    throw new Exception("Select a room.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}
