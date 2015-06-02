using System;
using System.IO;
using System.Linq;

namespace SdfEncryptor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 2)
                Usage();

            var filename = args[0];
            var newPassword = args[1];
            if (newPassword.Equals("empty", StringComparison.InvariantCultureIgnoreCase))
                newPassword = String.Empty;

            var oldPassword = String.Empty;
            if (args.Count() == 3)
                oldPassword = args[2];

            if (File.Exists(filename) == false)
                Usage($"Unable to find file {filename}");

            CompactAndProtectSqlCeFile(filename, newPassword, oldPassword);
        }

        static void CompactAndProtectSqlCeFile(string fileName, string newPassword, string oldPassword = "")
        {
            using (var sqlCeEngine = new System.Data.SqlServerCe.SqlCeEngine(ConnectionString(fileName, oldPassword)))
            { 
                Console.WriteLine($"Compacting new database ({fileName}, {new FileInfo(fileName).Length} bytes). Please wait as this may take several minutes.");
                sqlCeEngine.Compact(null);
                sqlCeEngine.Compact(ConnectionString(fileName, newPassword));
                Console.WriteLine($"Done. Database size is now {new FileInfo(fileName).Length} and encrypted.");
            }
        }

        static string ConnectionString(string sqlceDbFilePath, string oldPassword = null)
        {
            var connectionString = $"Data Source=\"{sqlceDbFilePath}\";Max Database Size=4000";
            if (oldPassword.HasValue())
            {
                connectionString = connectionString + $";password=\"{oldPassword}\";Encryption Mode=Engine Default";
            }
            return connectionString;
        }

        static void Usage(string message = "")
        {
            string usage = $"Usage: SdfEncryptor.exe [path] [newpassword] [oldpassword]. {message}";
            Console.Error.WriteLine(usage);
            Environment.Exit(1);
        }
    }

    public static class StringExtensions
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
