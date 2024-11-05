using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CineGt.DBCineGt;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CineGt.Forms
{
    public partial class buySeats : Form
    {
        private List<string> asientosSeleccionados = new List<string>();
        private int numeroDeAsientosPermitidos;
        DBCineGt db;
        string Username;

        int IdTransaction, IdMovieSession, numSeats, Room;

        public buySeats(int idTransaction, int idMovieSession, int numberSeats, string username, int room)
        {
            InitializeComponent();
            db = new DBCineGt();
            Username = username;
            dataGridView1.DataSource = null;
            ConfigurarDataGridView();
            var listaAsientos = db.ObtenerAsientos(idMovieSession);
            db.LlenarDataGridViewConAsientos(dataGridView1, listaAsientos);
            IdTransaction = idTransaction;
            IdMovieSession = idMovieSession;
            numSeats = numberSeats;
            Room = room;
            label6.Text = idTransaction.ToString();
            label7.Text = idMovieSession.ToString();
            label8.Text = numberSeats.ToString();
            numeroDeAsientosPermitidos = numberSeats;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            db.closeTicketTransaction(int.Parse(label6.Text), int.Parse(label7.Text));
            this.Hide();
        }

        private void buySeats_Load(object sender, EventArgs e)
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

                // Verificar si ya se ha seleccionado el asiento (para permitir deselección)
                if (asientosSeleccionados.Contains(asientoSeleccionado))
                {
                    asientosSeleccionados.Remove(asientoSeleccionado);
                    dataGridView1[colIndex, rowIndex].Style.BackColor = Color.White; // Cambia el color para deseleccionar
                }
                else
                {
                    // Verificar si se ha alcanzado el límite de selección
                    if (asientosSeleccionados.Count < numeroDeAsientosPermitidos)
                    {
                        asientosSeleccionados.Add(asientoSeleccionado);
                        dataGridView1[colIndex, rowIndex].Style.BackColor = Color.Blue; // Cambia el color para indicar selección
                    }
                    else
                    {
                        MessageBox.Show($"You can only select up to {numeroDeAsientosPermitidos} seats.", "Selection Limit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private string MostrarAsientosSeleccionados()
        {
            if (asientosSeleccionados.Count() != numSeats)
            {
                throw new Exception("Select all the seats.");
            }
            // Crear una lista de objetos anónimos para los asientos seleccionados
            var listaAsientosJson = asientosSeleccionados.Select(asiento => new
            {
                TicketsTransaction = IdTransaction,
                MovieSession = IdMovieSession,
                Seat = asiento,
                Room = Room
            }).ToList();

            // Convertir la lista a JSON
            string json = JsonSerializer.Serialize(listaAsientosJson);
            return json;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string json = MostrarAsientosSeleccionados();
                db.buyTickes(json);
                if (numSeats > 1)
                {
                    MessageBox.Show("The tickets were bought successfully.", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The ticket was bought successfully.", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
