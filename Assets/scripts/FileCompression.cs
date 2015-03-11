using SevenZip.Compression.LZMA;
using System;
using System.IO;
using System.Text;

public static class FileCompression
{
    public static string _file_compression_tempare = "_mtd_temp_compression";
    private static string _header_definition = "mtd-2014";

    public static void Compress(string src, string dst)
    {
        if (File.Exists(src))
        {
            Encoder encoder = new Encoder();
            FileStream inStream = new FileStream(src, FileMode.Open);
            FileStream outStream = new FileStream(_file_compression_tempare, FileMode.Create);
            try
            {
                outStream.Write(Encoding.ASCII.GetBytes(_header_definition), 0, 8);
                outStream.Write(TypeConvertUtil.intToBytes((int) inStream.Length, true), 0, 4);
                encoder.WriteCoderProperties(outStream);
                encoder.Code(inStream, outStream, -1L, -1L, null);
            }
            finally
            {
                inStream.Close();
                outStream.Close();
            }
            if (File.Exists(dst))
            {
                File.Delete(dst);
            }
            File.Move(_file_compression_tempare, dst);
        }
    }

    public static void Decompress(string src, string dst)
    {
        if (File.Exists(src))
        {
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
            FileStream inStream = new FileStream(src, FileMode.Open);
            FileStream outStream = new FileStream(_file_compression_tempare, FileMode.Create);
            try
            {
                byte[] array = new byte[8];
                byte[] buffer2 = new byte[4];
                byte[] buffer3 = new byte[5];
                inStream.Read(array, 0, 8);
                inStream.Read(buffer2, 0, 4);
                inStream.Read(buffer3, 0, 5);
                int num = TypeConvertUtil.bytesToint(buffer2, 0, true);
                decoder.SetDecoderProperties(buffer3);
                decoder.Code(inStream, outStream, inStream.Length, (long) num, null);
            }
            finally
            {
                inStream.Close();
                outStream.Close();
            }
            if (File.Exists(dst))
            {
                File.Delete(dst);
            }
            File.Move(_file_compression_tempare, dst);
        }
    }
}

