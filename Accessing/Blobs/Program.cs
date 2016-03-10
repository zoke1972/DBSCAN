using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blobs
{
    class Program
    {
        static byte[] ReadImage(string filePath)
        {
            FileStream stream = new FileStream(filePath,
            FileMode.Open);
            byte[] blob = new byte[stream.Length];
            int count = stream.Read(blob, 0, (int)stream.Length);
            Console.WriteLine(" Count: {0}", count);
            stream.Close();
            return blob;
        }

        static void Main(string[] args)
        {

            string connectionStr =
                                    @"Data Source=localhost;
                                    Initial Catalog=Memberships;
                                    Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionStr);
            String insert =
            @"INSERT INTO Members VALUES (41,'Bob', 'Bobic', @photo)";
            String query =
            @"SELECT photo FROM Members WHERE memberId = 40";
            String delete =
            @"DELETE FROM Members WHERE memberId = 40";
            try
            {

                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = insert;
                byte[] blob = ReadImage(@"C:\Users\zvule_000\Desktop\ZORAJA SVE\ZORAJA_PDFovi\BAZE_sql_2010\DB07_resources\Lipotica.jpg");
                command.Parameters.AddWithValue("@photo", blob);
                int count = (int)command.ExecuteNonQuery();
                Console.WriteLine(" Photo: {0}", count);
                blob = null;
                command.CommandText = query;
                blob = (byte[])command.ExecuteScalar();
                //command.CommandText = delete;
                //count = command.ExecuteNonQuery();
                Console.WriteLine(" {0} rows affected.", count);
               
            }
            catch (SqlException x)
            {
                Console.WriteLine(x.Message);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }
    }
}
