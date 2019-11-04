using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;

namespace PruebaEPAS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SqlConnection conn = new SqlConnection();
        private void Form1_Load(object sender, EventArgs e)
        {
            Llenar_Grid();
        }

        /// <summary>
        /// Verifica que las filas del DataTable vayan en montos incrementales.
        /// </summary>
        /// <param name="dt">DataTable a consultar</param>
        /// <returns>Un booleano indicando true cuando es en incrimento, de lo contrario false.</returns>
        private bool VerificarEnIncremento(DataTable dt, CarroCompra carrocompraEntrante)
        {
            bool resultado;
            float monto;
            monto = dt.Rows[0].Field<float>("MontoTotal");
            resultado = false;
            if (dt != null)
            {
                foreach (DataRow fila in dt.Rows)
                {
                    if (monto <= float.Parse(fila[4].ToString()))
                    {
                        monto = float.Parse(fila[4].ToString());
                        resultado = true;
                    }
                    else
                    {
                        resultado = false;
                        break;
                    }

                }
            }
            return resultado;
        }

        /// <summary>
        /// Verifica que las filas del DataTable vayan en montos en decremento
        /// </summary>
        /// <param name="dt">DataTable a consultar</param>
        /// <returns>Un booleano indicando false cuando es en decremento, de lo contrario true.</returns>
        private bool VerificarEnDecremento(DataTable dt, CarroCompra carrocompraEntrante)
        {
            bool resultado = false;

            try
            {

                bool hayrechazados;
                bool esmontomenor;
                hayrechazados = true;
                esmontomenor = false;
                // Condicion para saber si hayrechazados es igual a falso y
                //si no verificar que los monto de las transacciones son menores que los anteriores.
                if (dt == null)
                {
                    hayrechazados = false;
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0].Field<float>("MontoTotal") >= carrocompraEntrante.Carrocomprapago.Montototal)
                        {
                            esmontomenor = true;
                        }
                    }
                    foreach (DataRow fila in dt.Rows)
                    {

                        if (fila.Field<int>("Estatus") != (int)Estatus.Rechazado)
                        {
                            hayrechazados = false;
                        }
                    }
                }

                if (hayrechazados == true && esmontomenor == true)
                {
                    resultado = true;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("No se encontro la variable en el archivo de configuración");

            }
            return resultado;
        }

        private bool EPA1(CarroCompra carroCompraEntrante)
        {
            bool resultado;
            resultado = false;

            try
            {
                // SP que utilizare para la ejecucion de parametros
                SqlCommand command = new SqlCommand("EPA1", conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                //Envió los parámetros que necesito para el EPA1
                SqlParameter param1 = new SqlParameter("@userID", SqlDbType.Int);
                param1.Value = carroCompraEntrante.Cliente.Id;
                command.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("FechaTransaccion", SqlDbType.DateTimeOffset);
                param2.Value = carroCompraEntrante.Carrocomprapago.FechaUTCtransaccion;
                command.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter("@tarjetaPAN", SqlDbType.VarChar, 19);
                param3.Value = carroCompraEntrante.Carrocomprapago.Tarjetapan;
                command.Parameters.Add(param3);

                String EPA1TiempoDeVentana;
                EPA1TiempoDeVentana = ConfigurationManager.AppSettings.Get("EPA1TiempoDeVentana");

                int EPA1NumTransaccionesMin;
                EPA1NumTransaccionesMin = Int32.Parse(ConfigurationManager.AppSettings.Get("EPA1NumTransaccionesMin"));

                SqlParameter param4 = new SqlParameter("@Tiempo", SqlDbType.Int);
                param4.Value = Int32.Parse(EPA1TiempoDeVentana);
                command.Parameters.Add(param4);

                SqlParameter param5 = new SqlParameter("@NumRegistros", SqlDbType.Int);
                param5.Value = EPA1NumTransaccionesMin;
                command.Parameters.Add(param5);

                DataTable dt = new DataTable();

                conn.Open();

                //Aquí ejecuto el SP y lo lleno en el DataTable
                adapter.Fill(dt);
                resultado = VerificarEnDecremento(dt, carroCompraEntrante);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return resultado;
        }

        private bool EPA2(CarroCompra carroCompraEntrante)
        {
            bool resultado;
            resultado = false;


            try
            {
                SqlCommand command = new SqlCommand("EPA2", conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                //Envió los parámetros que necesito para el EPA2
                SqlParameter param01 = new SqlParameter("@userID", SqlDbType.Int);
                param01.Value = carroCompraEntrante.Cliente.Id;
                command.Parameters.Add(param01);

                SqlParameter param02 = new SqlParameter("FechaTransaccion", SqlDbType.DateTimeOffset);
                param02.Value = carroCompraEntrante.Carrocomprapago.FechaUTCtransaccion;
                command.Parameters.Add(param02);

                SqlParameter param03 = new SqlParameter("@tarjetaPAN", SqlDbType.VarChar, 19);
                param03.Value = carroCompraEntrante.Carrocomprapago.Tarjetapan;
                command.Parameters.Add(param03);

                String EPA2TiempoDeVentana;
                EPA2TiempoDeVentana = ConfigurationManager.AppSettings.Get("EPA2TiempoDeVentana");

                int EPA2NumTransaccionesMin;
                EPA2NumTransaccionesMin = Int32.Parse(ConfigurationManager.AppSettings.Get("EPA2NumTransaccionesMin"));

                SqlParameter param04 = new SqlParameter("@Tiempo", SqlDbType.Int);
                param04.Value = Int32.Parse(EPA2TiempoDeVentana);
                command.Parameters.Add(param04);

                SqlParameter param05 = new SqlParameter("@NumRegistros", SqlDbType.Int);
                param05.Value = EPA2NumTransaccionesMin;
                command.Parameters.Add(param05);

                DataTable dt = new DataTable();

                conn.Open();

                //Aquí ejecuto el SP y lo lleno en el DataTable
                adapter.Fill(dt);
                resultado = VerificarEnDecremento(dt, carroCompraEntrante);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                conn.Close();
            }
            return resultado;
        }

        private bool EPA3(CarroCompra carroCompraEntrante)
        {
            bool resultado;
            resultado = false;

            try
            {
                SqlCommand command = new SqlCommand("EPA3", conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                //Envió los parámetros que necesito para el EPA2
                SqlParameter param01 = new SqlParameter("@userID", SqlDbType.Int);
                param01.Value = carroCompraEntrante.Cliente.Id;
                command.Parameters.Add(param01);

                SqlParameter param02 = new SqlParameter("FechaTransaccion", SqlDbType.DateTimeOffset);
                param02.Value = carroCompraEntrante.Carrocomprapago.FechaUTCtransaccion;
                command.Parameters.Add(param02);

                SqlParameter param03 = new SqlParameter("@tarjetaPAN", SqlDbType.VarChar, 19);
                param03.Value = carroCompraEntrante.Carrocomprapago.Tarjetapan;
                command.Parameters.Add(param03);

                String EPA3TiempoDeVentana;
                EPA3TiempoDeVentana = ConfigurationManager.AppSettings.Get("EPA3TiempoDeVentana");

                int EPA3NumTransaccionesMin;
                EPA3NumTransaccionesMin = Int32.Parse(ConfigurationManager.AppSettings.Get("EPA3NumTransaccionesMin"));

                SqlParameter param04 = new SqlParameter("@Tiempo", SqlDbType.Int);
                param04.Value = Int32.Parse(EPA3TiempoDeVentana);
                command.Parameters.Add(param04);

                SqlParameter param05 = new SqlParameter("@NumRegistros", SqlDbType.Int);
                param05.Value = EPA3NumTransaccionesMin;
                command.Parameters.Add(param05);

                DataTable dt = new DataTable();

                conn.Open();

                //Aquí ejecuto el SP y lo lleno en el DataTable
                adapter.Fill(dt);
                resultado = VerificarEnDecremento(dt, carroCompraEntrante);

                dataGridView1.DataSource = dt;

            }
            catch (Exception)
            {

                throw;
            }
            return resultado;

        }

        private void Llenar_Grid()
        {
            // Los argumentos de la conexion de la base de datos
            string args = "Data Source = localhost; Initial Catalog = ProyectoResidencias; Integrated Security = True";
            conn = new SqlConnection();
            conn.ConnectionString = args;

            try
            {
                String TiempoDeVentana;
                TiempoDeVentana = ConfigurationManager.AppSettings.Get("EPA1TiempoDeVentana");

                //Indico el SP EPA1
                SqlCommand command = new SqlCommand("EPA1", conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                
                //Indico el SP Epa2 
                SqlCommand command1 = new SqlCommand("EPA2", conn);
                command1.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
                
                //Envió los parámetros que necesitopara el EPA1
                SqlParameter param1 = new SqlParameter("@userID", SqlDbType.Int);
                param1.Value = 00003;
                command.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("FechaTransaccion", SqlDbType.DateTimeOffset);
                param2.Value = DateTimeOffset.Parse("2019-10-16 9:45:00-5");
                command.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter("@tarjetaPAN", SqlDbType.VarChar, 19);
                param3.Value = "5474846151371020";
                command.Parameters.Add(param3);
                
                String EPA1TiempoDeVentana;
                EPA1TiempoDeVentana = ConfigurationManager.AppSettings.Get("EPA1TiempoDeVentana");

                int EPA1NumTransaccionesMin;
                EPA1NumTransaccionesMin = Int32.Parse(ConfigurationManager.AppSettings.Get("EPA1NumTransaccionesMin"));

                SqlParameter param4 = new SqlParameter("@Tiempo", SqlDbType.Int);
                param4.Value = Int32.Parse(EPA1TiempoDeVentana);
                command.Parameters.Add(param4);

                SqlParameter param5 = new SqlParameter("@NumRegistros", SqlDbType.Int);
                param5.Value = EPA1NumTransaccionesMin;
                command.Parameters.Add(param5);
                
                //Envió los parámetros que necesito para el EPA2
                SqlParameter param01 = new SqlParameter("@userID", SqlDbType.Int);
                param01.Value = 00002;
                command1.Parameters.Add(param01);

                SqlParameter param02 = new SqlParameter("FechaTransaccion", SqlDbType.DateTimeOffset);
                param02.Value = DateTimeOffset.Parse("2019-10-21 10:40:00-5");
                command1.Parameters.Add(param02);

                SqlParameter param03 = new SqlParameter("@tarjetaPAN", SqlDbType.VarChar, 19);
                param03.Value = "5474846151371020";
                command1.Parameters.Add(param03);
                
                String EPA2TiempoDeVentana;
                EPA2TiempoDeVentana = ConfigurationManager.AppSettings.Get("EPA2TiempoDeVentana");

                int EPA2NumTransaccionesMin;
                EPA2NumTransaccionesMin = Int32.Parse(ConfigurationManager.AppSettings.Get("EPA2NumTransaccionesMin"));

                SqlParameter param04 = new SqlParameter("@Tiempo", SqlDbType.Int);
                param04.Value = Int32.Parse(EPA2TiempoDeVentana);
                command1.Parameters.Add(param04);

                SqlParameter param05 = new SqlParameter("@NumRegistros", SqlDbType.Int);
                param05.Value = EPA2NumTransaccionesMin;
                command1.Parameters.Add(param05);

                DataTable dt = new DataTable();

                conn.Open();

                //Aquí ejecuto el SP y lo lleno en el DataTable
                adapter.Fill(dt);
                adapter1.Fill(dt);

                //Enlazo mis datos obtenidos en el DataTable con el grid
                dataGridView1.DataSource = dt;
            }
            // Excepciones con mensajes para determinar el nivel de error durante el procedimiento
            catch (ArgumentNullException)
            {
                MessageBox.Show("input is null.");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("The offset is greater than 14 hours or less than -14 hours.");
            }

            catch (FormatException)
            {
                MessageBox.Show("input does not contain a valid string representation of a date and time. or" +
                    " input contains the string representation of an offset value without a date or time.");
            }
            catch (OverflowException)
            {
                MessageBox.Show("s represents a number less than MinValue or greater than MaxValue.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
