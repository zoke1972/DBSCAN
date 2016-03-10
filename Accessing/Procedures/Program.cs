using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procedures
{
    class Program
    {
        static void Main(string[] args)
        {
            String connectionStr =
                                    @"Data Source=localhost;
                                    Initial Catalog=Town;
                                    Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionStr);

            try
            {
                connection.Open();
                SqlCommand command =
                new SqlCommand("TransferMoney", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@amount", 100);
                command.Parameters.AddWithValue("@from", 3);
                command.Parameters.AddWithValue("@to", 1);
                SqlParameter result =
                command.Parameters.Add("@result", SqlDbType.Int);
                result.Direction = ParameterDirection.Output;
                int count = command.ExecuteNonQuery();
                Console.WriteLine("Count: " + count);
                Console.WriteLine("Result: " + result.Value);

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
