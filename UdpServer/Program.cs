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
            // Ustawienia serwera
            int port = 11000;
            UdpClient server = new UdpClient(port);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);

            Console.WriteLine("Serwer UDP nasłuchuje na porcie " + port);

            try
            {
                while (true)
                {
                    // Odbieranie danych
                    byte[] receivedBytes = server.Receive(ref remoteEP);
                    string receivedData = Encoding.ASCII.GetString(receivedBytes);

                    // Deserializacja wiadomości
                    var message = JsonSerializer.Deserialize<Message>(receivedData);
                    Console.WriteLine($"Otrzymano wiadomość od klienta: {message.Content}, Timestamp: {message.Timestamp}");

                    // Wysyłanie odpowiedzi
                    var responseMessage = new Message { Content = "Hello from server", Timestamp = DateTime.Now };
                    string responseJson = JsonSerializer.Serialize(responseMessage);
                    byte[] responseBytes = Encoding.ASCII.GetBytes(responseJson);
                    server.Send(responseBytes, responseBytes.Length, remoteEP);
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
    }

    public class Message
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
