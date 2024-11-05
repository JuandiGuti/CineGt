using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CineGt.DBCineGt;

namespace CineGt.Forms
{
    public partial class deploySeats : Form
    {
        DBCineGt db;
        int Room;
        public deploySeats(int idTransaction, int idSession, string actualSeat, int room)
        {
            InitializeComponent();
            db = new DBCineGt();
            Room = room;
            dataGridView1.DataSource = null;
            ConfigurarDataGridView();
            var listaAsientos = db.ObtenerAsientos(idSession);
            db.LlenarDataGridViewConAsientos(dataGridView1, listaAsientos);
            label6.Text = idTransaction.ToString();
            label7.Text = idSession.ToString();
            label8.Text = actualSeat.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeSeat changeSeat = new ChangeSeat();

            this.Hide();
            changeSeat.ShowDialog();
        }

        private void deploySeats_Load(object sender, EventArgs e)
        {

        }
        private void ConfigurarDataGridView()
        {
            // Limpiar columnas y filas existentes
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            // Configurar columnas (1 a 10)
            for (int i = 1; i <= 10; i++)
            {
                DataGridViewImageColumn col = new DataGridViewImageColumn();
                col.HeaderText = i.ToString();
                col.Name = "Column" + i;
                col.Width = 50; // Ajusta el ancho según lo necesario
                dataGridView1.Columns.Add(col);
            }

            // Configurar filas (A a J)
            for (char rowHeader = 'A'; rowHeader <= 'J'; rowHeader++)
            {
                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].HeaderCell.Value = rowHeader.ToString();
            }

            // Ajustar la visualización de la fila y columna de encabezados
            dataGridView1.RowHeadersWidth = 50;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null) // Asegura que hay una celda seleccionada
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                int colIndex = dataGridView1.CurrentCell.ColumnIndex;

                // Verificar si la celda tiene el tag de "occupied"
                if (dataGridView1[colIndex, rowIndex].Tag?.ToString() == "occupied")
                {
                    MessageBox.Show("This seat is already occupied. Please select another one.", "Seat Occupied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Convertir el índice de fila en una letra (A-J)
                char fila = (char)('A' + rowIndex);
                int columna = colIndex + 1;

                // Crear el identificador del asiento
                string asientoSeleccionado = $"{fila}{columna}";

                label9.Text = asientoSeleccionado;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                db.cambiarAsiento(int.Parse(label6.Text), int.Parse(label7.Text), label8.Text, label9.Text, Room);
                MessageBox.Show($"The seat: {label8.Text} was changed for the seat: {label9.Text}.", "Seat Changed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
