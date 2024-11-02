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
            dataGridView1.DataSource =  db.llenarDGmovieSession();
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
    }
}
