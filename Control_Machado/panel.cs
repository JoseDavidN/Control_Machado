using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_Machado
{
    public partial class frm_panel : Form
    {
        // Crear una instancia de la clase ServerConnection
        ServerConnection server = new ServerConnection();

        public frm_panel()
        {
            InitializeComponent();
        }

        private void frm_panel_Load(object sender, EventArgs e)
        {
            // Iniciar el servidor
            Thread server_thread = new Thread(new ThreadStart(server.server_start));
            server_thread.Start();

            //while (true)
            //{
            //    // Mostrar la respuesta del servidor (mensaje cuando un cliente se conecta)
            //    string response = server.response;
            //    if (!string.IsNullOrEmpty(response))
            //    {
            //        label1.Text = response;
            //        server.response = null;  // Limpiar la respuesta para que no se muestre más de una vez
            //    }

            //    // Esperar un poco antes de verificar de nuevo
            //    Thread.Sleep(1000);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.message_send = "Hola desde el servidor";
            server.status_send = true;

            label1.Text = server.response;
        }

        private void frm_panel_FormClosed(object sender, FormClosedEventArgs e)
        {
            server.closeConnection();
        }
    }
}
