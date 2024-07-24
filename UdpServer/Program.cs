using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace UdpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 11000;
            UdpClient server = new UdpClient(port);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);

            Console.WriteLine("Serwer UDP nasłuchuje na porcie " + port);

            try
            {
                while (true)
                {
                    byte[] receivedBytes = server.Receive(ref remoteEP);
                    string receivedData = Encoding.ASCII.GetString(receivedBytes);

                    var message = JsonSerializer.Deserialize<Message>(receivedData);
                    Console.WriteLine($"Otrzymano komunikat: {message.Command}");

                    switch (message.Command)
                    {
                        case "ShowHelloWorld":
                            HandleShowHelloWorld(server, remoteEP, message);
                            break;
                        case "SendXaml":
                            HandleSendXaml(server, remoteEP, message);
                            break;
                        case "ShowDialog":
                            HandleShowDialog(server, remoteEP, message);
                            break;
                        case "ShowError":
                            HandleShowError(server, remoteEP, message);
                            break;
                        case "RequestProductConfirmation":
                            ForwardToTargetClient(message);
                            break;
                        case "AllProductsConfirmed":
                            ForwardToClientMaui(message);
                            break;
                        case "LoggedIn":
                            HandleLoggedIn(server, remoteEP, message);
                            break;
                        case "OptionOne":
                            HandleOptionOne(server, remoteEP, message);
                            break;
                        default:
                            Console.WriteLine($"Nieznany komunikat: {message.Command}");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystąpił błąd: " + e.ToString());
            }
            finally
            {
                server.Close();
            }
        }

        private static void HandleOptionOne(UdpClient server, IPEndPoint remoteEP, Message message)
        {
            ForwardToTargetClient(message);
        }

        private static void HandleSendXaml(UdpClient server, IPEndPoint remoteEP, Message message)
        {
            Console.WriteLine("Otrzymano plik XAML, przekazywanie do klienta docelowego");
            ForwardToTargetClient(message);
        }

        private static void HandleShowHelloWorld(UdpClient server, IPEndPoint remoteEP, Message message)
        {
            Console.WriteLine($"Przekazywanie wiadomości ShowHelloWorld do klienta docelowego");
            ForwardToTargetClient(message);
        }

        private static void HandleShowDialog(UdpClient server, IPEndPoint remoteEP, Message message)
        {
            Console.WriteLine($"Przekazywanie wiadomości ShowDialog do klienta docelowego");
            ForwardToTargetClient(message);
        }

        private static void HandleShowError(UdpClient server, IPEndPoint remoteEP, Message message)
        {
            Console.WriteLine($"Przekazywanie wiadomości ShowError do klienta docelowego");
            ForwardToTargetClient(message);
        }

        private static void HandleLoggedIn(UdpClient server, IPEndPoint remoteEP, Message message)
        {
            Console.WriteLine($"Przekazywanie wiadomości LoggedIn do klienta docelowego");
            ForwardToTargetClient(message);
        }

        private static void ForwardToClientMaui(Message message)
        {
            string targetClientAddress = "127.0.0.1";
            int targetClientPort = 12001; 


            using (UdpClient client = new UdpClient())
            {
                try
                {
                    string messageJson = JsonSerializer.Serialize(message);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);
                    client.Send(messageBytes, messageBytes.Length, targetClientAddress, targetClientPort);
                    Console.WriteLine($"Przekazano wiadomość {message.Command} do klienta wysyłającego na porcie {targetClientPort}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Błąd przekazywania wiadomości do klienta wysyłającego: " + e.ToString());
                }
            }
        }

        private static void ForwardToTargetClient(Message message)
        {
            string targetClientAddress = "127.0.0.1";
            int targetClientPort = 11001;

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    string messageJson = JsonSerializer.Serialize(message);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);
                    client.Send(messageBytes, messageBytes.Length, targetClientAddress, targetClientPort);
                    Console.WriteLine($"Przekazano wiadomość {message.Command} do klienta docelowego na porcie {targetClientPort}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Błąd przekazywania wiadomości do klienta docelowego: " + e.ToString());
                }
            }
        }
    }

    public class Message
    {
        public string Command { get; set; }
        public object Data { get; set; }
    }
}
