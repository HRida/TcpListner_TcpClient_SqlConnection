using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Net;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The Server is Run And Wait .....");

            IPAddress address = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(address, 7110);
            listener.Start();

            while (true)  // to make the server always on
            {
                TcpClient client = listener.AcceptTcpClient();                // to connect with clients

                NetworkStream clientStream = client.GetStream();
                byte[] message = new byte[4096];
                clientStream.Read(message, 0, message.Length);

                string messageFromClient = Encoding.ASCII.GetString(message, 0, message.Length);

                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = "Data Source=.;Initial Catalog=Northwind;Integrated Security=True";
                sqlConnection.Open();
                SqlDataAdapter adb = new SqlDataAdapter("Select EmployeeId From Employees Where LastName='" + messageFromClient.Trim('\0') + "'", sqlConnection);
                System.Data.DataSet dat = new System.Data.DataSet();
                adb.Fill(dat);

                string MessageToClient = dat.Tables[0].Rows[0].ItemArray[0].ToString();

                NetworkStream ServerStream = client.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(MessageToClient);
                ServerStream.Write(buffer, 0, buffer.Length);

                ServerStream.Flush();
                client.Close();

            }                
         //   listener.Stop();
        }
    }
}
