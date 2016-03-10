using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectDB
{
    class Program
    {
        public static string CreateInsertCommand(Member member)
        {
            string insert = @"INSERT Members VALUES (17, " +
            "'" + member.GetLastName() + "'" + ", " +
            "'" + member.GetFirstName() + "'" + ", NULL)";
            return insert;
        }
        static void DisplayResult(SqlDataReader reader)
        {
            Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine("Output Encoding: " +
            Console.OutputEncoding);
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader.GetName(i) + " ");
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
            String connectionStr =
                                    @"Data Source=localhost;
                                    Initial Catalog=Memberships;
                                    Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionStr);
            string query = @"SELECT memberId, lastName, firstName FROM Members";
            Member member = Member.ReadMember();
            if (member == null) return;
            string modify = CreateInsertCommand(member);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = modify;
                int count = command.ExecuteNonQuery();
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
