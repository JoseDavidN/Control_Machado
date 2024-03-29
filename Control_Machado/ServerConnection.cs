using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Control_Machado
{
    internal class ServerConnection
    {
        public string response { get; set; }
        public string message_send { get; set; }
        public bool status_send { get; set; }

        private Socket Server;
        private Socket Client;
        private IPAddress Ip = IPAddress.Parse("127.0.0.1");
        private int Port = 1649;
        private SocketType SockType;
        private ProtocolType SockProtocol;
        private byte[] receiveBuffer, sendBuffer;
        private IPEndPoint iPEndPoint;

        public void server_start()
        {
            // Definicion del tipo de protocolo y socket
            SockType = SocketType.Stream;
            SockProtocol = ProtocolType.Tcp;
            // Configuracion de los buffers
            receiveBuffer = new byte[4096];
            sendBuffer = new byte[4096];

            // Creacion del socket
            Server = new Socket(AddressFamily.InterNetwork, SockType, SockProtocol);

            // Configuracion del punto de conexion
            iPEndPoint = new IPEndPoint(Ip, Port);

            try
            {
                // Enlazar el socket al punto de conexion
                Server.Bind(iPEndPoint);
                // Escuchar por conexiones
                Server.Listen(1);

                while (true)
                {
                    // Aceptar la conexion
                    Client = Server.Accept();

                    response = "Conexion establecida con " + Client.RemoteEndPoint.ToString();

                    // Crear un hilo para recibir y enviar datos
                    Thread client_thread = new Thread(new ParameterizedThreadStart(client_start));
                    client_thread.IsBackground = true;
                    client_thread.Start(Client);
                }
            }
            catch (Exception e)
            {
                response = e.Message;
            }
        }

        private void client_start(object clientObj)
        {
            Socket client = (Socket)clientObj;

            try
            {
                while (true)
                {
                    // Recibir datos
                    var dataRecibe = client.Receive(receiveBuffer);

                    try
                    {
                        // Convertir los datos a string
                        response = Encoding.UTF8.GetString(receiveBuffer, 0, dataRecibe);
                    }
                    catch (Exception e)
                    {
                        response = e.Message;
                    }

                    if ((message_send != null) && (status_send == true))
                    {
                        try
                        {
                            sendBuffer = Encoding.ASCII.GetBytes(message_send);
                            client.Send(sendBuffer);
                            status_send = false;
                        }
                        catch (Exception e)
                        {
                            response = e.Message;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                response = e.Message;
            }
        }

        public void closeConnection()
        {
            Server.Close();
            message_send = null;
            status_send = false;
            response = null;
            //Client.Close();
        }
    }
}
