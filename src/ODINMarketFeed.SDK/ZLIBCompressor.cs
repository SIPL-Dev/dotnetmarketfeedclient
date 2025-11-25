using System;
using System.Collections.Generic;
using System.Text;
using FTIL.Compression.ZLib;

namespace PriceFeedAPI
{
   
	public class ZLIBCompressor 
    {    
        public  byte[] Compress(byte[] toComp)
        {
            byte[] sbData = (byte[])toComp;
            byte[] Comp = null;
            ZStream oCompresszStream = new ZStream();

            byte[] sbOutData = new byte[sbData.Length * 10];
            if (oCompresszStream == null)
                oCompresszStream = new ZStream();

            int err = oCompresszStream.deflateInit(zlibConst.Z_DEFAULT_COMPRESSION);
            // CHECK_ERR(zstream, err, "deflate");

            oCompresszStream.next_in = sbData;
            oCompresszStream.next_in_index = 0;

            oCompresszStream.next_out = sbOutData;
            oCompresszStream.next_out_index = 0;

            while (oCompresszStream.total_in != sbData.Length && oCompresszStream.total_out < sbOutData.Length)
            {
                oCompresszStream.avail_in = oCompresszStream.avail_out = 1; // force small buffers
                err = oCompresszStream.deflate(zlibConst.Z_NO_FLUSH);
                if (err != 0)
                {

                }
                // CHECK_ERR(zstream, err, "deflate");
            }

            while (true)
            {
                if (oCompresszStream.total_out < oCompresszStream.next_out.Length)
                {

                }
                else
                {
                    break;
                }
                oCompresszStream.avail_out = 1;
                err = oCompresszStream.deflate(zlibConst.Z_FINISH);
                if (err == zlibConst.Z_STREAM_END) break;
                // CHECK_ERR(zstream, err, "deflate");
                if (err != 0)
                {

                }

            }


            oCompresszStream.deflateEnd();

            byte[] sbFinalCompressedData = new byte[oCompresszStream.total_out];
            Buffer.BlockCopy(oCompresszStream.next_out, 0, sbFinalCompressedData, 0, sbFinalCompressedData.Length);

            oCompresszStream.free();
            Comp = sbFinalCompressedData;

            return Comp;//compression
            //return toComp;//plaintext


        }

        public byte[] Uncompress(byte[] toDcomp , out string Error)
        {
            byte[] sbComprData = (byte[])toDcomp;
            byte[] dComp = null;
            ZStream ozUnCompressStream = new ZStream();
            byte[] PendingBytes = null;
            int err = ozUnCompressStream.inflateInit();
            //  CHECK_ERR(d_stream, err, "inflate");

            ozUnCompressStream.next_in = sbComprData;


            ozUnCompressStream.next_in_index = 0;



            while (ozUnCompressStream.next_in_index < ozUnCompressStream.next_in.Length)
            {

                ozUnCompressStream.next_out_index = 0;
                ozUnCompressStream.total_out = 0;

                byte[] sbUnComprData = new byte[(ozUnCompressStream.next_in.Length - ozUnCompressStream.next_in_index) * 100];
                ozUnCompressStream.next_out = sbUnComprData;

                while (ozUnCompressStream.total_out < sbUnComprData.Length && (ozUnCompressStream.total_in < sbComprData.Length))
                {
                    //ozUnCompressStream.avail_in = sbComprData.Length;
                    //ozUnCompressStream.avail_out = sbUnComprData.Length;
                    ozUnCompressStream.avail_in = ozUnCompressStream.avail_out = 1; /* force small buffers */
                    err = ozUnCompressStream.inflate(zlibConst.Z_NO_FLUSH);

                    if (err == zlibConst.Z_STREAM_END || err != 0) break;
                    //  CHECK_ERR(d_stream, err, "inflate");
                }

                if (PendingBytes == null)
                {
                    PendingBytes = new byte[ozUnCompressStream.total_out];
                    Buffer.BlockCopy(ozUnCompressStream.next_out, 0, PendingBytes, 0, PendingBytes.Length);

                }
                else
                {
                    byte[] bytTemp = PendingBytes;
                    PendingBytes = new byte[bytTemp.Length + ozUnCompressStream.total_out];
                    Buffer.BlockCopy(bytTemp, 0, PendingBytes, 0, bytTemp.Length);
                    Buffer.BlockCopy(ozUnCompressStream.next_out, 0, PendingBytes, bytTemp.Length, PendingBytes.Length - bytTemp.Length);
                }

                if (err == zlibConst.Z_STREAM_END || err != 0) break;

            }

            err = ozUnCompressStream.inflateEnd();
            //  CHECK_ERR(d_stream, err, "inflate");

            dComp = PendingBytes;
            PendingBytes = null;
            ozUnCompressStream.free();
            Error = "";
            return dComp;
            //error = "SUCCESS";
            //return true;

        }

    }
}
