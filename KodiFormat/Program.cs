using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace KodiFormat
{
    class Program
    {
        static void Main(string[] args)
        {
            string source, destination;
            string defaultDir = @"E:\films";

            Console.Write("Source (" + defaultDir + ") : ");
            source = Console.ReadLine();
            if (String.IsNullOrEmpty(source))
            {
                source = defaultDir;
            }
            if (!Directory.Exists(source))
            {
                Console.WriteLine("Le repertoire source " + source + " n'existe pas\nexit !!");
                Console.Read();
                return;
            }

            Console.Write("Destination (" + defaultDir + ") : ");
            destination = Console.ReadLine();
            if (String.IsNullOrEmpty(destination))
            {
                destination = defaultDir;
            }
            if (!Directory.Exists(destination))
            {
                Console.WriteLine("Le repertoire de destination " + destination + " n'existe pas\nexit !!");
                Console.Read();
                return;
            }

            string[] fileEntries = Directory.GetFiles(source);
            string fileDirectory, fileName, basename;
            Regex regFileName = new Regex("( |_){1,}", RegexOptions.Multiline);
            Regex regFileExtension = new Regex(@"\.[a-z0-9]{2,4}$", RegexOptions.IgnoreCase);
            foreach (string file in fileEntries)
            {
                basename = Path.GetFileName(file);
                basename = regFileName.Replace(basename, ".");
                basename = regFileExtension.Replace(basename, "");

                fileDirectory = source + @"\" + basename;
                fileName = fileDirectory + @"\" + Path.GetFileName(file);
                if (!Directory.Exists(basename))
                {
                    try
                    {
                        Directory.CreateDirectory(fileDirectory);
                        Console.WriteLine("\t(CD)\t" + basename);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("\t(Error CD)\t" + basename + " (" + e.Message + ")");
                    }
                }
                if (!File.Exists(fileName))
                {
                    try
                    {
                        File.Move(file, fileName);
                        Console.WriteLine("\t(MF)\t" + fileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\t(Error MF)\t" + fileName + " (" + e.Message + ")");
                    }
                }
            }

            // Rename directory .
            string[] directoryEntries = Directory.GetDirectories(source, "*");
            Regex regDirectory = new Regex(@"\.", RegexOptions.IgnoreCase);
            Regex regYearDirectory = new Regex(@" ([0-9]{4})$", RegexOptions.IgnoreCase);
            foreach (string dir in directoryEntries)
            {
                string newName = regDirectory.Replace(dir, " ");
                newName = newName.Trim();
                newName = regYearDirectory.Replace(newName, " ($1)");
                if (dir != newName)
                {
                    try
                    {
                        Directory.Move(dir, newName);
                        Console.WriteLine("\t(RD)\t" + newName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\t(Error RD)\t" + newName + " (" + e.Message + ")");
                    }                    
                }
            }

            Console.WriteLine("Terminé...");
            Console.Read();
        }
    }
}
