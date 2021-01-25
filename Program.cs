using System;
using System.IO;


namespace pictureProcessor
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hi there! This program is a processor for ELI-16-bit-grayscale pictures  files. \nFirst, you must enter a name of FIRST ELI file, it must be placed in program home directory. \nSecond, you must enter a name of SECOND ELI FILE, it must be placed in program home directory. \nThis program will process files and save result File named FIRST_FILE_NAME_RESULT.ELI in same directory.\n\nPlease enter FIRST ELI  file name (with extension at the end) : ");
            string sFirstFileName = Directory.GetCurrentDirectory() + "\\" + Console.ReadLine();
            Console.WriteLine(" \nPlease enter SECOND ELI file name (with extension at the end) : ");
            string sSecondFileName = Directory.GetCurrentDirectory() + "\\" + Console.ReadLine();

            if (System.IO.File.Exists(sFirstFileName) && System.IO.File.Exists(sSecondFileName))
            {
                pictureProcessor picProc = new pictureProcessor(sFirstFileName,sSecondFileName);
                picProc.process();
            }
            else Console.WriteLine("File does not exist, please restart programm with correct file names"); 


        }
    }
}
