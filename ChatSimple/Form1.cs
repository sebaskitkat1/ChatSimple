using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Xsl;
using Microsoft.VisualBasic;
namespace ChatSimple
{
    public partial class Form1 : Form
    {
        private TcpClient cliente;
        private StreamReader reader;
        private StreamWriter writer;
        private Conexion conexion=new Conexion();

        private string nombreCliente="";

        private List<StreamWriter> clientesConectados = new List<StreamWriter>();

        private readonly object lockClientes = new object();

        private bool esServidor = false;
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Esta aplicacion " +
                "es el Servidor?", "Sistema", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            try
            {
                if (respuesta == DialogResult.Yes)
                {
                    esServidor |= true;
                    int puerto = int.Parse(txtPuerto.Text);
                    TcpListener listener = new TcpListener(IPAddress.Any, puerto);
                    listener.Start();



                    rtbHistorial.AppendText("Servidor iniciado en la direccion y puerto:  " + getIP() + ":" + puerto + "...\r\n");


                    // Bucle infinito: El servidor nunca deja de aceptar clientes
                    while (true)
                    {
                        TcpClient nuevoCliente = await listener.AcceptTcpClientAsync();
                        rtbHistorial.AppendText("¡Un nuevo cliente se ha unido a la sala!\r\n");

                        // Manejamos cada cliente en una tarea en segundo plano separada
                        _ = ManejarCliente(nuevoCliente);
                    }
                }
                else
                {
                    nombreCliente = Interaction.InputBox("Ingrese su nombre:", "Sistema", "Cliente");
                    string ip = txtIP.Text;
                    int port = int.Parse(txtPuerto.Text);

                    cliente = new TcpClient();
                    rtbHistorial.AppendText("Conectando al servidor...\r\n");

                    await cliente.ConnectAsync(ip, port);

                    conexion.cargarHistorial(rtbHistorial);

                    rtbHistorial.AppendText("¡Conectado a la sala como " + nombreCliente + "!\r\n");



                    NetworkStream stream = cliente.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream) { AutoFlush = true };

                    await writer.WriteLineAsync(nombreCliente);
                    _ = RecibirMensajes();

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }

        }

        private async Task ManejarCliente(TcpClient cliente)
        {
            NetworkStream stream = cliente.GetStream();
            StreamReader clientReader = new StreamReader(stream);
            StreamWriter clientWriter = new StreamWriter(stream) { AutoFlush = true };

            // Añadimos el nuevo cliente a nuestra lista segura
            lock (lockClientes) { clientesConectados.Add(clientWriter); }

            string nombreCliente = await clientReader.ReadLineAsync();

            try
            {
                while (cliente.Connected)
                {
                    // Escucha los mensajes de ESTE cliente en particular
                    string mensajeRecibido = await clientReader.ReadLineAsync();

                    if (mensajeRecibido != null)
                    {
                        // Mostrar en la pantalla del servidor
                        rtbHistorial.Invoke((MethodInvoker)delegate {
                            rtbHistorial.AppendText($"{nombreCliente}: {mensajeRecibido}\r\n");
                        });

                        conexion.ejecutarComando($"INSERT INTO historial (usuario, mensaje) VALUES ('{nombreCliente}', '{mensajeRecibido}')");

                        // Reenviar a todos los demás clientes de la sala
                        DifundirMensaje($"{nombreCliente}: {mensajeRecibido}", clientWriter);
                    }
                    else
                    {
                        break; // Si recibe null, el cliente se desconectó
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error con un cliente, posiblemente se desconectó o falló.");
            }
            finally
            {
                // Si el cliente se desconecta, lo quitamos de la lista
                lock (lockClientes) { clientesConectados.Remove(clientWriter); }
                rtbHistorial.Invoke((MethodInvoker)delegate {
                    rtbHistorial.AppendText($"{nombreCliente} abandono la sala.\r\n");
                });
                cliente.Close();
            }
        }

        private async void DifundirMensaje(string mensaje, StreamWriter excluir=null)
        {
            List<StreamWriter> copiaClientes;

            // Hacemos una copia rápida de la lista por seguridad
            lock (lockClientes) { copiaClientes = new List<StreamWriter>(clientesConectados); }

            foreach (var clientWriter in copiaClientes)
            {
                if (clientWriter == excluir) continue;
                try
                {
                    await clientWriter.WriteLineAsync(mensaje);
                }
                catch
                {

                }
            }
        }

        private string getIP()
        {
            string hostName = Dns.GetHostName();
            string myIP = "";
            IPHostEntry host = Dns.GetHostEntry(hostName);

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) // Filtra IPv4
                {
                    myIP = ip.ToString();
                    break;
                }
            }
            return myIP;
        }
        private void ConfigurarStreams()
        {
            NetworkStream stream = cliente.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
        }

        private async Task RecibirMensajes()
        {
            try
            {
                while (cliente != null && cliente.Connected)
                {
                    string mensajeRecibido = await reader.ReadLineAsync();
                    if (mensajeRecibido != null)
                    {
                        rtbHistorial.Invoke((MethodInvoker)delegate {
                            rtbHistorial.AppendText(mensajeRecibido + "\r\n");
                        });
                    }
                }


            }
            catch (Exception)
            {
                rtbHistorial.Invoke((MethodInvoker)delegate
                {
                    rtbHistorial.AppendText("Cliente " + nombreCliente + " Desconectado \n");
                });
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string mensaje = txtMensaje.Text;
            if (string.IsNullOrWhiteSpace(mensaje)) return;

            try
            {
                if (esServidor)
                {
                    // Si soy el servidor, muestro mi mensaje y lo difundo a todos
                    rtbHistorial.AppendText("Server Admin: " + mensaje + "\r\n");
                    DifundirMensaje("Server Admin: " + mensaje);

                    conexion.ejecutarComando($"INSERT INTO historial (usuario, mensaje) VALUES ('Server Admin', '{mensaje}')");
                }
                else if (cliente != null && cliente.Connected)
                {
                    // Si soy cliente, le mando mi mensaje al servidor (y él se encargará de repartirlo)
                    await writer.WriteLineAsync(mensaje);
                    rtbHistorial.AppendText("Tú: " + mensaje + "\r\n");
                }

                txtMensaje.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar: " + ex.Message);
            }
        }
    }
}