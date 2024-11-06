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
    public partial class loadCSVsession : Form
    {
        DBCineGt db;
        string? csvPath = null;
        public loadCSVsession()
        {
            InitializeComponent();
            db = new DBCineGt();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (csvPath == null)
                {
                    throw new Exception("Select a path.");
                }
                if (checkBox1.Checked)
                {
                    db.ProcessSessionsFromCSV(csvPath, 1);
                }
                else
                {
                    db.ProcessSessionsFromCSV(csvPath, 0);
                }
                MessageBox.Show("Finish.");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "CSV files (*.csv)|*.csv"; // Filtrar para archivos CSV
            openFileDialog.Title = "Seleccione un archivo CSV";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                csvPath = openFileDialog.FileName;

                label4.Text = "Path loaded.";
            }
        }
    }
}
