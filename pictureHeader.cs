using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace pictureProcessor
{
    public abstract class fileHeader
    {
       
    }

    public class pictureHeader :fileHeader,  IEquatable<pictureHeader>
    {
        public string signature { get; }
        public Int32 length { get; }
        public Int32 data_offset { get; }
        public Int32 reserved { get; }
        public Int32 image_width { get; }
        public Int32 image_height { get; }
        public Int32 bit_count { get; }
        public Int32 line_length { get; }
        public bool data_offset_is_multiple512 { get=> data_offset%512==0; }

        public Int32 number_of_lines { get; }

        public bool Equals(pictureHeader other)
        {
            return (other.signature == signature) &&(other.length == length) &&(other.data_offset == data_offset) && (other.reserved == reserved) && (other.image_width == image_width) && (other.image_height == image_height) && (other.bit_count == bit_count) && (other.line_length == line_length) && (data_offset_is_multiple512);
        }

        public override bool Equals(object other)
        {
            return Equals((pictureHeader)other);
        }

        public pictureHeader(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                Byte[] sig = new Byte[4];

                for (int i = 0; i < 4; i++)
                    sig[i] = reader.ReadByte();

                signature = System.Text.Encoding.Default.GetString(sig);

                length = reader.ReadInt32();

                data_offset = reader.ReadInt32();

                reserved = reader.ReadInt32();

                image_width = reader.ReadInt32();

                image_height = reader.ReadInt32();

                bit_count = reader.ReadInt32();

                line_length = reader.ReadInt32();

                number_of_lines = (Int32)(reader.BaseStream.Length - data_offset) / line_length;

            }
        }
    }
}
