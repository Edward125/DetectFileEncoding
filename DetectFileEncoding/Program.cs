using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DetectFileEncoding
{
    class Program
    { 
        static int   Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("DETECT TEXT FILE ENCODING CONSOLE APPLICATION");
                Console.WriteLine("  by Edward, 2017.03");
                Console.WriteLine("  read text file BOM(byte-order mark),and return encoding");
                Console.WriteLine();
                Console.WriteLine(@"  Usage: 'DetectFIleEncoding [text file path]");
                Console.WriteLine(@"  Sample usage: 'DetectFIleEncoding ""c:\1.txt""");
                return -1;

            }
            else
            {
                string filepath = @args[0].ToString();
                if (File.Exists(filepath))
                    Console.WriteLine(FindEncoding(filepath));
                return 0;

            }

           //Console.ReadKey();
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static string FindEncoding(string filename)
        {
            string result = "The Encoding is:";
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
                //show BOM
                Console.WriteLine(bom[0].ToString("X2") + " " + bom[1].ToString("X2") + " " + bom[2].ToString("X2") + " " + bom[3].ToString("X2"));
                
            }
            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return result + "UTF7";
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return result + "UTF8";
            if (bom[0] == 0xff && bom[1] == 0xfe) return result + "Unicode(UTF-16LE)"; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return result +"BigEndianUnicode(UTF-16BE)"; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return result + "UTF32";//Encoding.UTF32;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return result + "UTF-32(LE)"; // Encoding.GetEncoding("UTF-32(LE)");
            if (bom[0] == 0xf7 && bom[1] == 0x64 && bom[2] == 0x4c) return result + "UTF-1"; //Encoding.GetEncoding("UTF-1");
            if (bom[0] == 0xdd && bom[1] == 0x73 && bom[2] == 0x66 && bom[3] ==0x73) return result + "UTF-EBCDIC";
            if (bom[0] == 0x0e && bom[1] == 0xfe && bom[2] == 0xff ) return result + "SCSU";
            if (bom[0] == 0xfb && bom[1] == 0xee && bom[2] == 0x28 ) return result + "BOCU-1";
            if (bom[0] == 0x84 && bom[1] == 0x31 && bom[2] == 0x95 && bom[3] == 0x33) return result + "GB-18030";
            return result + "ASCII";
            
         
        }

    }
}
