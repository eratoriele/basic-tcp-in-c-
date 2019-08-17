using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class serv
{
    public static void Main()
    {
        try
        {
            IPAddress ipAd = IPAddress.Parse("192.168.10.62");
            // use local m/c IP address, and 
            // use the same in the client

            /* Initializes the Listener */
            TcpListener myList = new TcpListener(ipAd, 1000);

            /* Start Listeneting at the specified port */
            myList.Start();
            
            Console.WriteLine("The local End point is:" + myList.LocalEndpoint);
            Console.WriteLine("Waiting for a connection");

            Socket s = myList.AcceptSocket();
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            string exitMessage = string.Empty;
            while (!exitMessage.Equals("exit")) {

                byte[] b = new byte[100];
                int k = s.Receive(b);
                Console.WriteLine("Recieved");
                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(b[i]));

                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("The string was recieved by the server.\n"));
                Console.WriteLine("\nSent Acknowledgement");

                exitMessage = Encoding.Default.GetString(b, 0, 4);
            }
            /* clean up */
            s.Close();
            myList.Stop();

        }
        catch (Exception e)
        {
            Console.WriteLine("Error..... " + e.StackTrace);
        }
    }

}