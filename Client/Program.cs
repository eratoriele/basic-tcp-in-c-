using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class ClientAndServer
{

    // args = [ip, port, server/client]
    public static void Main(string[] args)
    {

        if (args.Length != 3)
        {
            Console.WriteLine("args fields are as follows:\n0: ip address\n1:port\n2:Client/Server, 0 for Client, 1 for Server");
            return;
        }

        switch (Int32.Parse(args[2]))
        {
            case 0:
                Client(args[0], Int32.Parse(args[1]));
                break;

            case 1:
                Server(args[0], Int32.Parse(args[1]));
                break;

            default:
                Console.WriteLine("args fields are as follows:\n0: ip address\n1:port\n2:Client/Server, 0 for Client, 1 for Server");
                break;
        }

    }

    private static void Client(string ip, int port)
    {
        try
        {
            TcpClient tcpclnt = new TcpClient();
            Console.WriteLine("Connecting");

            tcpclnt.Connect(ip, port);
            // use the ipaddress as in the server program

            Console.WriteLine("Connected");

            String str = String.Empty;
            while (!str.Equals("exit"))
            {

                // Writing
                Console.Write("Enter the string to be transmitted : ");

                str = Console.ReadLine();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);

                Stream stm = tcpclnt.GetStream();

                Console.WriteLine("Transmitting");
                stm.Write(ba, 0, ba.Length);

                /*
                 * 
                // Reading
                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(bb[i]));
                    */
            }

            tcpclnt.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
    }


    private static void Server(string ip, int port)
    {
        try
        {
            IPAddress ipAd = IPAddress.Parse(ip);
            // use local m/c IP address, and 
            // use the same in the client

            /* Initializes the Listener */
            TcpListener myList = new TcpListener(ipAd, port);

            /* Start Listeneting at the specified port */
            myList.Start();

            Console.WriteLine("The local End point is:" + myList.LocalEndpoint);
            Console.WriteLine("Waiting for a connection");

            Socket s = myList.AcceptSocket();
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            string exitMessage = string.Empty;
            while (!exitMessage.Equals("exit"))
            {

                // Reading
                byte[] b = new byte[100];
                int k = s.Receive(b);
                Console.WriteLine("Recieved");
                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(b[i]));

                // Sending back
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