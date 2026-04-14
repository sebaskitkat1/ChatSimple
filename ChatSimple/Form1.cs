using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatSimple
{
    public partial class Form1 : Form
    {
        private TcpClient cliente;
        private StreamReader reader;
        private StreamWriter writer;
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Esta aplicacion es el Servidor?", "Sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            try
            {
                if (result == DialogResult.Yes)
                {
                    int port = int.Parse(txtPuerto.Text);
                    TcpListener server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    rtbHistorial.AppendText("Servidor iniciado en el puerto: "+ port + "\n");

                    // Esperar a que un cliente se conecte de manera asíncrona
                    cliente = await server.AcceptTcpClientAsync();
                    rtbHistorial.AppendText("Cliente conectado!\n");

                    ConfigurarStreams();
                    _=RecibirMensajes();
                } else
                {
                    string ip = txtIP.Text; ;
                    int port = int.Parse(txtPuerto.Text);
                    cliente = new TcpClient();
                    rtbHistorial.AppendText("Conectando al servidor...\n");
                    await cliente.ConnectAsync(ip, port);
                    rtbHistorial.AppendText("Conectado!\n");
                    ConfigurarStreams();
                    _=RecibirMensajes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar el servidor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string getIp()
        {
            string hostName = Dns.GetHostName();
            string ip = "";
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
            foreach (IPAddress ipAddress in hostEntry.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = ipAddress.ToString();
                    break;
                }
            }
            return ip;
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
                        rtbHistorial.Invoke((MethodInvoker)delegate
                        {
                            rtbHistorial.AppendText("Extraño: " + mensajeRecibido + "\n");
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                rtbHistorial.Invoke((MethodInvoker)delegate
                {
                    rtbHistorial.AppendText("Cliente desconectado!\n");
                });
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (cliente != null && cliente.Connected && !string.IsNullOrWhiteSpace(txtMensaje.Text))
            {
                try
                {
                    string mensaje=txtMensaje.Text;
                    await writer.WriteLineAsync(mensaje);
                    rtbHistorial.AppendText("Yo: " + mensaje + "\n");
                    txtMensaje.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al enviar el mensaje: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No hay clientes conectados", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
