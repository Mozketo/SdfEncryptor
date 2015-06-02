using System;
using System.IO;
using System.Linq;

namespace SdfEncryptor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 2)
                Usage();

            var filename = args[0];
            var password = args[1];

            if (File.Exists(filename) == false)
                Usage($"Unable to find file {filename}");

            CompactAndProtectSqlCeFile(filename, password);
        }

        static void CompactAndProtectSqlCeFile(string fileName, string password)
        {
            using (var sqlCeEngine = new System.Data.SqlServerCe.SqlCeEngine(ConnectionString(fileName)))
            { 
                Console.WriteLine($"Compacting new database ({fileName}, {new FileInfo(fileName).Length} bytes). Please wait as this may take several minutes.");
                sqlCeEngine.Compact(null);

                if (password.HasValue())
                {
                    Console.WriteLine($"Encrypting with Password. This will take several minutes.");
                    sqlCeEngine.Compact(ConnectionString(fileName, password));
                    Console.WriteLine($"Database size is now {new FileInfo(fileName).Length} and encrypted.");
                }
            }
        }

        static string ConnectionString(string sqlceDbFilePath, string password = null)
        {
            var connectionString = $"Data Source=\"{sqlceDbFilePath}\";Max Database Size=4000";
            if (password.HasValue())
            {
                connectionString = connectionString + $";password=\"{password}\";Encryption Mode=Engine Default";
            }
            return connectionString;
        }

        static void Usage(string message = "")
        {
            string usage = $"Usage: SdfEncryptor.exe [path] [key]. {message}";
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
