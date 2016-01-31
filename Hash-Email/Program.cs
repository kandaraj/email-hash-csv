using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hash_Email
{
    class Program
    {
        static void Main(string[] args)
        {
            var lineno = 1;
            var sha1 = new SHA1CryptoServiceProvider();
            Console.WriteLine("Hashing email from file");
            StringBuilder content = new StringBuilder();
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead("08Dec15-21Dec15.csv"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var newline = line;

                    var matches = Regex.Matches(
                        line,
                        @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
                        RegexOptions.IgnoreCase);
 
                    foreach (Match match in matches)
                    {
                        var email = Encoding.UTF8.GetBytes(match.Value);
                        var hashed = Convert.ToBase64String(sha1.ComputeHash(email));                        
                        newline = newline.Replace(match.Value, hashed.ToString());                        
                    }
                    Console.WriteLine(lineno);
                    lineno += 1;
                    content.AppendLine(newline);                    
                }
                
            }           

            Console.WriteLine("Done.");
            Console.Read();

           File.WriteAllText(@"newfile-hashed.txt", content.ToString());
        }


    }
}
