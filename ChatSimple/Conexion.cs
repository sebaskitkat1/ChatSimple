using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace ChatSimple
{
    internal class Conexion
    {
        string cadenaConexion = "server=Localhost;port=3307;user=sebas;pwd=Luna115115;Database=chat";
        MySqlConnection conexion;
        public void conectar()
        {
            try
            {
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
            }
        }

        public void desconectar()
        {
            try
            {
                if (conexion != null)
                    conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desconectar de la base de datos: " + ex.Message);
            }
        }

        public void ejecutarComando(string comando)
        {
            try
            {
                conectar();
                MySqlCommand cmd = new MySqlCommand(comando, conexion);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar el comando: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                desconectar();
            }
        }

        public void cargarHistorial(RichTextBox rtb)
        {
            try
            {
                conectar();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM historial ORDER BY fecha ASC", conexion);
                MySqlDataReader reader = cmd.ExecuteReader();
                rtb.AppendText("Historial de mensajes:\r\n");
                while (reader.Read())
                {
                    string nombre = reader.GetString("usuario");
                    string mensaje = reader.GetString("mensaje");
                    string fecha = reader.GetDateTime("fecha").ToString("yyyy-MM-dd HH:mm:ss");
                    rtb.AppendText(nombre + ": " + mensaje + " (" + fecha + ")\r\n");
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el historial: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                desconectar();
            }
        }
    }
}
