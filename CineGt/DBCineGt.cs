using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CineGt
{
    internal class DBCineGt
    {
        private string connectionString = "Data Source=LAPTOP-E3AMJ72E\\SQLEXPRESS;Initial Catalog=CineGt;User=CineGtAppUser;Password=CineGtAppUser;";
        public bool Ok()
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void userRegister(string username, string password /*LA PASS TIENE QUE VENIR YA HASHEADA AQUI*/)
        {
            string query = "NewUser";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERNAME", SqlDbType.VarChar, 100)).Value = username;
                    cmd.Parameters.Add(new SqlParameter("@PASS", SqlDbType.VarChar, 300)).Value = password;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public (bool, int) userLogIn(string username, string password)
        {
            string query = "UserLogin";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERNAME", SqlDbType.VarChar, 100)).Value = username;

                    SqlParameter passwordParam = new SqlParameter("@PASSWORD", SqlDbType.VarChar, 300);
                    passwordParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(passwordParam);

                    SqlParameter roleParam = new SqlParameter("@ROLE", SqlDbType.Int);
                    roleParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(roleParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    string hashedPassword = passwordParam.Value.ToString();
                    int role = int.Parse(roleParam.Value.ToString());

                    conn.Close();

                    bool passCheck = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

                    return (passCheck, role);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void llenarComboBox(System.Windows.Forms.ComboBox combobox, string column, string table)
        {
            string query = $"SELECT {column} FROM {table};";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        using (SqlCommand command = new SqlCommand(query, conn, transaction))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                combobox.Items.Clear();

                                while (reader.Read())
                                {
                                    combobox.Items.Add(reader[column].ToString());
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void newMovie(string movieName, string movieDescription, TimeSpan duration, string classification)
        {
            string query = "NewMovie"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.VarChar, 100)).Value = movieName;
                    cmd.Parameters.Add(new SqlParameter("@DURATION", SqlDbType.Time)).Value = duration;
                    cmd.Parameters.Add(new SqlParameter("@DESCRIPTION", SqlDbType.VarChar, 500)).Value = movieDescription;
                    cmd.Parameters.Add(new SqlParameter("@CLASIFICATION", SqlDbType.VarChar, 20)).Value = classification;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void newSession(DateTime beginningDate, string movieName, int room, string username)
        {
            string query = "NewMovieSession"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@BEGININGDATE", SqlDbType.DateTime)).Value = beginningDate;
                    cmd.Parameters.Add(new SqlParameter("@MOVIENAME", SqlDbType.VarChar, 100)).Value = movieName;
                    cmd.Parameters.Add(new SqlParameter("@ROOM#", SqlDbType.Int)).Value = room;
                    cmd.Parameters.Add(new SqlParameter("@APPUSER", SqlDbType.VarChar, 100)).Value = username;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<movieSession> llenarDGmovieSession()
        {
            List<movieSession> list = new List<movieSession>();
            string query = "SELECT MS.ID AS SessionId, MS.BeginningDate, MS.EndingDate, M.MovieName, MS.Room FROM MovieSession MS, Movie M WHERE SessionState = 0  AND BeginningDate > GETDATE() AND MS.Movie = M.ID;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                     
                        while (reader.Read())
                        {
                            movieSession movieSession = new movieSession();
                            movieSession.id = reader.GetInt32(0);
                            movieSession.beginningDate = reader.GetDateTime(1);
                            movieSession.endingDate = reader.GetDateTime(2);
                            movieSession.movieName = reader.GetString(3);
                            movieSession.room = reader.GetInt32(4);
                            list.Add(movieSession);
                        }
                        reader.Close();
                        conn.Close();
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void cancelSession(int movieSessionId)
        {
            string query = "CancelMovieSession"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MOVIESESSIONID", SqlDbType.Int)).Value = movieSessionId;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<changeSeat> llenarDGchangeSeat(string email)
        {
            List<changeSeat> list = new List<changeSeat>();
            string query = "SearchSeatsByClient";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@EMAIL", SqlDbType.VarChar, 50)).Value = email;
                       
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            changeSeat changeSeat = new changeSeat();
                            changeSeat.transactionId = reader.GetFieldValue<int>(0);
                            changeSeat.movieSessionId = reader.GetFieldValue<int>(1);
                            changeSeat.transactionCreateDate = reader.GetFieldValue<DateTime>(2);
                            changeSeat.transactionModificationDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3);
                            changeSeat.seat = reader.GetFieldValue<string>(4);
                            changeSeat.movieName = reader.GetFieldValue<string>(5);

                            list.Add(changeSeat);
                        }
                        reader.Close();
                        conn.Close();
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Seat> ObtenerAsientos(int movieSessionId)
        {
            List<Seat> lista = new List<Seat>();
            string query = "SELECT * FROM SeatByMovieSession WHERE MovieSession = @MovieSession";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MovieSession", movieSessionId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Seat asiento = new Seat
                    {
                        id = reader.GetInt32(0),
                        available = reader.GetInt32(1),
                        movieSession = reader.GetInt32(2),
                        seat = reader.GetString(3),
                        ticketsTransaction = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4)
                    };
                    lista.Add(asiento);
                }
                conn.Close();
            }
            return lista;
        }
        public void LlenarDataGridViewConAsientos(DataGridView dataGridView1, List<Seat> listaAsientos)
        {
            // Cargar las imágenes de los asientos
            string disponibleImgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "available.png");
            string ocupadoImgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "occupied.png");

            Image disponibleImg = Image.FromFile(disponibleImgPath);
            Image ocupadoImg = Image.FromFile(ocupadoImgPath);

            // Recorrer la lista de asientos y actualizar el DataGridView
            foreach (var asiento in listaAsientos)
            {
                int rowIndex = asiento.seat[0] - 'A'; // Convertir la letra de fila a índice
                int colIndex = int.Parse(asiento.seat.Substring(1)) - 1; // Convertir el número de columna a índice

                // Asignar la imagen y el tag según el estado de disponibilidad
                dataGridView1[colIndex, rowIndex].Value = asiento.available == 1 ? disponibleImg : ocupadoImg;
                dataGridView1[colIndex, rowIndex].Tag = asiento.available == 1 ? "available" : "occupied"; // Asigna el tag
            }
        }
        public void cambiarAsiento(int idTicketTransaction, int idMovieSession, string actualSeat, string newSeat)
        {
            string query = "ChangeSeat"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    cmd.Parameters.Add(new SqlParameter("@TICKETSTRANSACTIONID", SqlDbType.Int)).Value = idTicketTransaction;
                    cmd.Parameters.Add(new SqlParameter("@MOVIESSESIONID", SqlDbType.Int)).Value = idMovieSession;
                    cmd.Parameters.Add(new SqlParameter("@ACTUALSEAT", SqlDbType.VarChar, 3)).Value = actualSeat;
                    cmd.Parameters.Add(new SqlParameter("@NEWSEAT", SqlDbType.VarChar, 3)).Value = newSeat;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public class movieSession
        {
            public int id { get; set; }
            public DateTime beginningDate { get; set; }
            public DateTime endingDate { get; set; }
            public int room { get; set; }
            public string movieName { get; set; }
        }
        public class changeSeat()
        {
            public int transactionId {  get; set; }
            public int movieSessionId { get; set; }
            public DateTime transactionCreateDate { get; set; }
            public DateTime? transactionModificationDate { get; set; }
            public string seat { get; set; }
            public string movieName { get; set; }
        }
        public class Seat
        {
            public int id { get; set; }
            public int available { get; set; }
            public int movieSession { get; set; }
            public string seat { get; set; }
            public int? ticketsTransaction { get; set; }
        }

    }
}
