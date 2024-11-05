using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CineGt.Forms
{
    public partial class report5 : Form
    {
        DBCineGt db;
        public report5()
        {
            InitializeComponent();
            db = new DBCineGt();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void report5_Load(object sender, EventArgs e)
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
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = db.llenarDGGetTop5Movies();
                dataGridView1.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}
