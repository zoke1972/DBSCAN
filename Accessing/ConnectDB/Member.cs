using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectDB
{
    public class Member
    {
        string lastName;
        string firstName;
        public Member(string lastName, string firstName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }
        public string GetLastName() { return lastName; }
        public string GetFirstName() { return firstName; }
        public override string ToString()
        {
            return firstName + ", " + lastName;
        }
        public static Member ReadMember()
        {
            Encoding oldEncoding = Console.InputEncoding;
            Console.InputEncoding = Encoding.GetEncoding(1250);
            Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine("Enter First Name: ");
            string firstName = Console.ReadLine();
            if (firstName != null) firstName.Trim();
            else return null;
            Console.WriteLine("Enter Last Name: ");
            string lastName = Console.ReadLine();
            if (lastName != null) lastName.Trim();
            else return null;
            Member member = new Member(lastName, firstName);
            Console.InputEncoding = oldEncoding;
            return member;
        }
    }
}
