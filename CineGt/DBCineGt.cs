using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CineGt
{
    internal class DBCineGt
    {
        private string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=BuenaCineGt;User=CineGtAppUser;Password=CineGtAppUser;";
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
            string query = "newAppUser";
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
            string query = "makeLogin";
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
            string query = "addMovie"; // Nombre del procedimiento almacenado
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
            string query = "addSession"; // Nombre del procedimiento almacenado
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
                    using (SqlCommand cmd = new SqlCommand(query, conn))
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
            string query = "cancelSession"; // Nombre del procedimiento almacenado
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
            string query = "ByClientSearchSeats";
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
                            changeSeat.idRoom = reader.GetFieldValue<int>(6);
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
            string query = @"
        SELECT sbms.ID, sbms.Available, sbms.MovieSession, s.seat, sbms.TicketsTransaction
        FROM SeatByMovieSession sbms
        INNER JOIN Seat s ON sbms.Seat = s.ID
        WHERE sbms.MovieSession = @MovieSession";

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
                        seat = reader.GetString(3),  // Obtenemos el nombre del asiento como string
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
        public void cambiarAsiento(int idTicketTransaction, int idMovieSession, string actualSeat, string newSeat, int room)
        {
            string query = "changeSeat"; // Nombre del procedimiento almacenado
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
                    cmd.Parameters.Add(new SqlParameter("@ROOM", SqlDbType.VarChar, 3)).Value = room;

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
        public List<sessionByMovie> llenarDGsessionByMovie(string movieName)
        {
            List<sessionByMovie> list = new List<sessionByMovie>();
            string query = $"SELECT MS.ID AS SessionId, MS.BeginningDate, MS.EndingDate, M.MovieName, MS.Room, (100 - MS.CompromisedSeats) AS AvailableSeats FROM MovieSession MS, Movie M WHERE SessionState = 0  AND BeginningDate > GETDATE() AND MS.Movie = M.ID AND M.MovieName = '{movieName}' ORDER BY BeginningDate ASC;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            sessionByMovie sessionByMovie = new sessionByMovie();
                            sessionByMovie.id = reader.GetInt32(0);
                            sessionByMovie.beginningDate = reader.GetDateTime(1);
                            sessionByMovie.endingDate = reader.GetDateTime(2);
                            sessionByMovie.movieName = reader.GetString(3);
                            sessionByMovie.room = reader.GetInt32(4);
                            sessionByMovie.availableSeats = reader.GetInt32(5);
                            list.Add(sessionByMovie);
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
        public void newClient(string name, string email, string phone, int age)
        {
            string query = "addClient"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.VarChar, 50)).Value = name;
                    cmd.Parameters.Add(new SqlParameter("@EMAIL", SqlDbType.VarChar, 50)).Value = email;
                    cmd.Parameters.Add(new SqlParameter("@PHONE", SqlDbType.VarChar, 12)).Value = phone;
                    cmd.Parameters.Add(new SqlParameter("@AGE", SqlDbType.Int)).Value = age;

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
        public int newTicketTransaction(string email, string username, int numberSeats, int idMovieSession)
        {
            double payment = 50 * numberSeats;

            string query = "startTicket"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@CLIENTEMAIL", SqlDbType.VarChar, 50)).Value = email;
                    cmd.Parameters.Add(new SqlParameter("@APPUSER", SqlDbType.VarChar, 50)).Value = username;
                    cmd.Parameters.Add(new SqlParameter("@PAYMENT", SqlDbType.VarChar, 12)).Value = payment;
                    cmd.Parameters.Add(new SqlParameter("@#SEATS", SqlDbType.Int)).Value = numberSeats;
                    cmd.Parameters.Add(new SqlParameter("@MOVIESESSIONID", SqlDbType.Int)).Value = idMovieSession;

                    SqlParameter idTransaction = new SqlParameter("@IDTRANSACTION", SqlDbType.Int);
                    idTransaction.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(idTransaction);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    int idTicketTransaction = int.Parse(idTransaction.Value.ToString());

                    conn.Close();
                    return idTicketTransaction;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void closeTicketTransaction(int idTicketTransaction, int idMovieSession)
        {
            string query = "closeTicket"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TICKETSTRANSACTIONID", SqlDbType.Int)).Value = idTicketTransaction;
                    cmd.Parameters.Add(new SqlParameter("@MOVIESESSIONID", SqlDbType.Int)).Value = idMovieSession;

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
        public void buyTickes(string json)
        {
            string query = "confirmTicket"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@json", SqlDbType.NVarChar)).Value = json;

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
        public void cancelTicketTransaction(int idTicketTransaction)
        {
            string query = "cancelTicket"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TICKETSTRANSACTIONID", SqlDbType.Int)).Value = idTicketTransaction;

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
        public List<SearchTransactionByClient> llenarDGcancelTransaction(string email)
        {
            List<SearchTransactionByClient> list = new List<SearchTransactionByClient>();
            string query = "ClientByTransaction";
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
                            SearchTransactionByClient cl = new SearchTransactionByClient();
                            cl.idTicketTransaction = reader.GetFieldValue<int>(0);
                            cl.createDate = reader.GetFieldValue<DateTime>(1);
                            cl.modDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(3);
                            cl.paymet = reader.GetFieldValue<decimal>(3);
                            cl.numseat = reader.GetFieldValue<int>(4);
                            cl.BeginningDateSession = reader.GetFieldValue<DateTime>(5);
                            cl.movieName = reader.GetFieldValue<string>(6);
                            cl.idMovieSession = reader.GetFieldValue<int>(7);
                            list.Add(cl);
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
        public List<GetOccupiedSeatsBySession> llenarDGGetOccupiedSeatsBySession(DateTime startDate, DateTime endDate)
        {
            List<GetOccupiedSeatsBySession> list = new List<GetOccupiedSeatsBySession>();
            string query = "GetOccupiedSeatsBySession";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)).Value = startDate;
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)).Value = endDate;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            GetOccupiedSeatsBySession cl = new GetOccupiedSeatsBySession
                            {
                                idMovieSession = reader.GetFieldValue<int>(0),
                                sessionState = reader.GetFieldValue<int>(1),
                                beginningDate = reader.GetFieldValue<DateTime>(2),
                                endingDate = reader.GetFieldValue<DateTime>(3),
                                compromisedSeats = reader.GetFieldValue<int>(4),
                                idMovie = reader.GetFieldValue<int>(5),
                                room = reader.GetFieldValue<int>(6),
                                appUser = reader.GetFieldValue<string>(7),
                                occupiedSeats = reader.GetFieldValue<int>(8)
                            };
                            list.Add(cl);
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
        public List<ListTicketTransactions> llenarDGListTicketTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<ListTicketTransactions> list = new List<ListTicketTransactions>();
            string query = "ListTicketTransactions"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)).Value = startDate;
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)).Value = endDate;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ListTicketTransactions transaction = new ListTicketTransactions
                            {
                                TransactionID = reader.GetFieldValue<int>(0),
                                TransactionStatus = reader.GetFieldValue<int>(1),
                                PurchaseDate = reader.GetFieldValue<DateTime>(2),
                                ModificationDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetFieldValue<DateTime>(3),
                                Payment = reader.GetFieldValue<decimal>(4),
                                ClientName = reader.GetFieldValue<string>(5),
                                Email = reader.GetFieldValue<string>(6),
                                Phone = reader.GetFieldValue<string>(7),
                                TotalSeats = reader.GetFieldValue<int>(8),
                                MovieSessionID = reader.GetFieldValue<int>(9),
                                SessionState = reader.GetFieldValue<int>(10),
                                BeginningDate = reader.GetFieldValue<DateTime>(11),
                                EndingDate = reader.GetFieldValue<DateTime>(12),
                                MovieName = reader.GetFieldValue<string>(13),
                                RoomID = reader.GetFieldValue<int>(14)
                            };
                            list.Add(transaction);
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
        public List<GetRoomOccupancyByMonth> llenarDGGetRoomOccupancyByMonth(int room)
        {
            List<GetRoomOccupancyByMonth> list = new List<GetRoomOccupancyByMonth>();
            string query = "GetRoomOccupancyByMonth"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoomID", SqlDbType.Int)).Value = room;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            GetRoomOccupancyByMonth transaction = new GetRoomOccupancyByMonth
                            {
                                year = reader.GetFieldValue<int>(0),
                                Month = reader.GetFieldValue<int>(1),
                                avgSeatsOccupied = reader.GetFieldValue<decimal>(2),
                                sessionCount = reader.GetFieldValue<int>(3)
                            };
                            list.Add(transaction);
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
        public List<GetLowOccupancySessions> llenarDGGetLowOccupancySessions(decimal percentage)
        {
            List<GetLowOccupancySessions> list = new List<GetLowOccupancySessions>();
            string query = "GetLowOccupancySessions"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@PercentageThreshold", SqlDbType.Decimal)).Value = percentage;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            GetLowOccupancySessions transaction = new GetLowOccupancySessions
                            {
                                id = reader.GetFieldValue<int>(0),
                                beginningDate = reader.GetFieldValue<DateTime>(1),
                                endingDate = reader.GetFieldValue<DateTime>(2),
                                movie = reader.GetFieldValue<int>(3),
                                room = reader.GetFieldValue<int>(4),
                                soldSeats = reader.GetFieldValue<int>(5),
                                OccupancyRate = reader.GetFieldValue<decimal>(6)
                            };
                            list.Add(transaction);
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
        public List<GetTop5Movies> llenarDGGetTop5Movies()
        {
            List<GetTop5Movies> list = new List<GetTop5Movies>();
            string query = "GetTop5Movies"; // Nombre del procedimiento almacenado
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            GetTop5Movies transaction = new GetTop5Movies
                            {
                                movieName = reader.GetFieldValue<string>(0),
                                avgSeatSoldPerSession = reader.GetFieldValue<Decimal>(1)
                            };
                            list.Add(transaction);
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



        public class movieSession
        {
            public int id { get; set; }
            public DateTime beginningDate { get; set; }
            public DateTime endingDate { get; set; }
            public int room { get; set; }
            public string movieName { get; set; }
        }
        public class changeSeat
        {
            public int transactionId { get; set; }
            public int movieSessionId { get; set; }
            public DateTime transactionCreateDate { get; set; }
            public DateTime? transactionModificationDate { get; set; }
            public string seat { get; set; }
            public string movieName { get; set; }
            public int idRoom { get; set; }
        }
        public class Seat
        {
            public int id { get; set; }
            public int available { get; set; }
            public int movieSession { get; set; }
            public string seat { get; set; }
            public int? ticketsTransaction { get; set; }
        }
        public class sessionByMovie
        {
            public int id { get; set; }
            public DateTime beginningDate { get; set; }
            public DateTime endingDate { get; set; }
            public int room { get; set; }
            public string movieName { get; set; }
            public int availableSeats { get; set; }
        }
        public class SearchTransactionByClient
        {
            public int idTicketTransaction { get; set; }
            public DateTime createDate { get; set; }
            public DateTime? modDate { get; set; }
            public decimal paymet { get; set; }
            public int numseat { get; set; }
            public DateTime BeginningDateSession { get; set; }
            public string movieName { get; set; }
            public int idMovieSession { get; set; }
        }
        public class GetOccupiedSeatsBySession
        {
            public int idMovieSession { get; set; }
            public int sessionState { get; set; }
            public DateTime beginningDate { get; set; }
            public DateTime endingDate { get; set; }
            public int compromisedSeats { get; set; }
            public int idMovie { get; set; }
            public int room { get; set; }
            public string appUser { get; set; }
            public int occupiedSeats { get; set; }
        }
        public class GetTransactionDetailsByDateRange
        {
            public int ID { get; set; }
            public int TransactionStatus { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime? ModificationDate { get; set; } // Nullable in case it might be null
            public decimal Payment { get; set; }
            public int Client { get; set; }
            public string AppUser { get; set; }
            public int NumberSeats { get; set; }
            public int Movie { get; set; }
            public int Room { get; set; }
            public DateTime BeginningDate { get; set; }
            public DateTime EndingDate { get; set; }
        }
        public class ListTicketTransactions
        {
            public int TransactionID { get; set; }
            public int TransactionStatus { get; set; }
            public DateTime PurchaseDate { get; set; }
            public DateTime? ModificationDate { get; set; } // Nullable in case it might be null
            public decimal Payment { get; set; }
            public string ClientName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public int TotalSeats { get; set; }
            public int MovieSessionID { get; set; }
            public int SessionState { get; set; }
            public DateTime BeginningDate { get; set; }
            public DateTime EndingDate { get; set; }
            public string MovieName { get; set; }
            public int RoomID { get; set; }
        }
        public class GetRoomOccupancyByMonth
        {
            public int year { get; set; }
            public int Month { get; set; }
            public decimal avgSeatsOccupied { get; set; }
            public int sessionCount { get; set; }
        }
        public class GetLowOccupancySessions
        {
            public int id { get; set; }
            public DateTime beginningDate { get; set; }
            public DateTime endingDate { get; set; }
            public int movie { get; set; }
            public int room { get; set; }
            public int soldSeats { get; set; }
            public decimal OccupancyRate { get; set; }
        }
        public class GetTop5Movies
        {
            public string movieName { get; set; }
            public decimal avgSeatSoldPerSession { get; set; }
        }
    }
}
