using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace pictureProcessor
{
    public class pictureProcessor
    {
        public pictureHeader picHeader;

        public string firstFileName;
        public string secondFileName;
        public string resFileName;

        private List<byte[]> firstPicture = new List<byte[]>();
        private List<byte[]> secondPicture = new List<byte[]>();
        private List<byte[]> resultPicture = new List<byte[]>();

        public pictureProcessor(string f1, string f2) 
        {
            firstFileName = f1;
            secondFileName = f2;
            picHeader = new pictureHeader(f1);
            pictureHeader picHeader2 = new pictureHeader(f2);

            if (picHeader.Equals(picHeader2))
            {
                Console.WriteLine("\nData from header: \nSiganature: {0}  \nHeader length : {1}  \nOffset: {2} \nReserved Field: {3}  \nPicture width: {4} \nPicture height:{5} \nBit per pixel:{6} \nByte in line:{7} \nnumber of lines:{8}",
                     picHeader.signature, picHeader.length, picHeader.data_offset, picHeader.reserved, picHeader.image_width, picHeader.image_height, picHeader.bit_count, picHeader.line_length, picHeader.number_of_lines);

                resFileName = f1.Substring(0, f1.LastIndexOf(".")) + "_RESULT" + f1.Substring(f1.LastIndexOf("."));

            }
            else Console.WriteLine("Files cannot be processed because they have different or incorrect headers.");

        }

        private byte[] getFileHeader()
        {
            if (picHeader != null)
            {
                byte[] header = new byte[picHeader.data_offset];

                using (FileStream fstream = File.OpenRead(firstFileName))
                {                    
                    fstream.Read(header, 0, picHeader.data_offset);
                }

                return header;
            }
            else return null;
        }


        private void readPictureData(string fName, ref List<byte[]> storageList)
        {
            using (FileStream fstream = File.OpenRead(fName))
            {
                Int32 currentReaderPosition = picHeader.data_offset;

                fstream.Seek(currentReaderPosition, SeekOrigin.Begin);

                while (currentReaderPosition < fstream.Length)
                {
                    byte[] pictureRaw = new byte[picHeader.line_length];
                    fstream.Read(pictureRaw, 0, picHeader.line_length);
                    currentReaderPosition += picHeader.line_length;
                    storageList.Add(pictureRaw);
                }

            }
        }

        private byte[] divisionPicturePixelLine(byte[] pic1, byte [] pic2)
        {
            List<byte> result = new List<byte>();

            int position=0;

            while (position < picHeader.line_length)
            {
                
                result = result.Concat(BitConverter.GetBytes((UInt16)(BitConverter.ToUInt16(pic1, position) / BitConverter.ToUInt16(pic2, position)))).ToList();
                position += 2;
            }

            return result.ToArray();
        }

        private void saveResultPicture()
        {
            using (FileStream fstream = new FileStream(resFileName, FileMode.Create))
            {
             
                fstream.Write(getFileHeader(), 0, picHeader.data_offset);
                foreach(byte[] picLine in resultPicture)
                {
                    fstream.WriteAsync(picLine, 0, picHeader.line_length);
                }                

                Console.WriteLine("\nResult saved to file "+ resFileName);
            }
        }
    

        public void process()
        {
            if (picHeader != null)
            {
                readPictureData(firstFileName, ref firstPicture);
                readPictureData(secondFileName, ref secondPicture);

                var tempByteArrayCollection = firstPicture.AsParallel().AsOrdered().Zip(secondPicture.AsParallel().AsOrdered(), (a, b) => divisionPicturePixelLine(a,b));

                resultPicture = tempByteArrayCollection.ToList();

                saveResultPicture();
            }
        }


    }
}
