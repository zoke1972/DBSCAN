using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Connecting
{
    class Program
    {
        static void DisplayResult(SqlDataReader reader)
        {
            Console.OutputEncoding = new UTF8Encoding();
            //Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine("Output Encoding: " +
            Console.OutputEncoding);
            for (int i = 0; i < reader.FieldCount; i++)
                //Console.Write(reader.GetName(i) + " ");
            Console.WriteLine();
            while (reader.Read())
            {
                Console.WriteLine("{0,8} {1,6} {2, 8}",
                reader["memberId"],
                reader["lastName"],
                reader["firstName"]);
            }
        }
        static void Main(string[] args)
        {

            String connectionStr = @"Data Source=localhost;
                                    Initial Catalog=Memberships;
                                    Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionStr);
            String query = @"SELECT memberId, lastName, firstName FROM Members";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                DisplayResult(reader);
                reader.Close();
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
