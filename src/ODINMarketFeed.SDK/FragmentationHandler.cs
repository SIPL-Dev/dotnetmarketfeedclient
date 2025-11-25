using FTIL.Compression.ZLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceFeedAPI
{
    public class FragmentationHandler
    {
        private MemoryStream m_memoryStream;
        private BinaryReader m_binaryReader;
        private int m_LastWrittenIndex;

        #region :Variables
        
        private const int _MinimumPacketSize = 5;
        private const int _PacketHeaderSize = 5;
        private bool m_isDisposed = false;
        
        public bool isDisposed
        {
            get { return m_isDisposed; }
            set { m_isDisposed = value; }
        }
        
        protected ZLIBCompressor ZlibCompressor;
        public const byte MESSAGELENGTH_LENGTH = 5;
       

        #endregion

        #region :Properties
        public MemoryStream memoryStream
        {
            get { return m_memoryStream; }
        }
        
        public BinaryReader binaryReader
        {
            get { return m_binaryReader; }
        }

        public int LastWrittenIndex
        {
            get { return m_LastWrittenIndex; }
            set { m_LastWrittenIndex = value; }
        }
        #endregion :Properties

        #region :Constructor
        public FragmentationHandler()
        {
            ZlibCompressor = new ZLIBCompressor();
            m_memoryStream = new MemoryStream();
            m_binaryReader = new BinaryReader(m_memoryStream, System.Text.Encoding.ASCII);
            m_LastWrittenIndex = -1;
        }
        #endregion :Constructor

        public byte[] FragmentData(byte[] bytData)
        {
            byte[] buffer = null;


            buffer = ZlibCompressor.Compress(bytData);
            string LengthString = "";
            LengthString = buffer.Length.ToString().PadLeft(6, '0');
            byte[] lenBytes = new byte[6];
            lenBytes = System.Text.Encoding.ASCII.GetBytes(LengthString);

            lenBytes[0] = 5;//compression
            byte[] BytesToSent = new byte[6 + buffer.Length]; ;
            Buffer.BlockCopy(lenBytes, 0, BytesToSent, 0, lenBytes.Length);
            Buffer.BlockCopy((Array)buffer, 0, BytesToSent, 6, buffer.Length);

            return BytesToSent;
        }

        public ArrayList DeFragment(byte[] data)
        {
            if (isDisposed)
                return null;

            lock (memoryStream)
            {
                memoryStream.Position = LastWrittenIndex + 1;
                memoryStream.Write(data, 0, data.Length);
                LastWrittenIndex = (int)memoryStream.Position - 1;

                return DefragmentData();

            }
        }

        private ArrayList DefragmentData()
        {
            bool parseDone;
            int bytesParsed;
            int iPacketSize;
            ArrayList packetList;

            parseDone = false;
            bytesParsed = 0;
            iPacketSize = 0;
            memoryStream.Position = 0;
            packetList = new ArrayList();

            while (memoryStream.Position < LastWrittenIndex - _MinimumPacketSize &&
                    !parseDone)
            {

                iPacketSize = isLength(binaryReader.ReadChars(_PacketHeaderSize + 1));

                if (iPacketSize <= 0)
                {
                    memoryStream.Position = memoryStream.Position - _PacketHeaderSize + 1;
                    bytesParsed++;
                }
                else
                {
                    if (memoryStream.Position + iPacketSize <= LastWrittenIndex + 1)
                    {
                        DefragmentInnerData(memoryStream.Position, binaryReader.ReadBytes(iPacketSize), ref packetList);
                        bytesParsed += _PacketHeaderSize + 1 + iPacketSize;
                    }
                    else
                        parseDone = true;
                }
            }

            ClearProcessedData(bytesParsed);
            return packetList;
        }

        private int isLength(char[] header)
        {


            int iLength = 0;

            if (header.Length != _PacketHeaderSize + 1)
                return -1;

            if (header[0] == (byte)5 || header[0] == (byte)2)
            {
                for (int i = 1; i < _PacketHeaderSize + 1; i++)
                {
                    if (!char.IsDigit(header[i]))
                        return -1;
                }
            }
            else
                return -1;
            string sLength = new string(header, 1, 5);
            iLength = Convert.ToInt32(sLength);

            return iLength;
        }
       
        private int DefragmentInnerData(long StreamPosition, byte[] CompressData, ref ArrayList packetList)
        {
            object objMessageData;
            byte[] MessageData;
            byte[] packetData;
            int Pos, iPacketSize, packetCount = 0;
            string sPacketSize;
            string errString = "";
            objMessageData = null;

            MessageData = ZlibCompressor.Uncompress(CompressData, out errString);
            while (true)
            {
                m_UnCompressMsgLength = 0;
                m_UnCompressMsgLength = GetMessageLength(MessageData);

                if (m_UnCompressMsgLength <= 0)
                {
                    MessageData = null;
                    break;
                }

                byte[] unCompressBytes = new byte[m_UnCompressMsgLength];
                Buffer.BlockCopy(MessageData, _HeaderLength, unCompressBytes, 0, m_UnCompressMsgLength);

                packetList.Add(unCompressBytes);
                packetCount++;
                byte[] unCompressNewBytes = new byte[MessageData.Length - m_UnCompressMsgLength - _HeaderLength];

                if (unCompressNewBytes.Length <= 0)
                {
                    MessageData = null;
                    unCompressNewBytes = null;
                    break;
                }

                Buffer.BlockCopy(MessageData, m_UnCompressMsgLength + _HeaderLength, unCompressNewBytes, 0, (MessageData.Length - (m_UnCompressMsgLength + _HeaderLength)));

                MessageData = unCompressNewBytes;
            }

            Pos = 0;
            iPacketSize = 0;
            packetCount = 0;
            //packetList.Add(MessageData);


            return packetCount;
        }

        private void ClearProcessedData(int length)
        {
            byte[] data;
            int size;
            if (length <= 0)
                return;


            if (length >= LastWrittenIndex + 1)
            {
                LastWrittenIndex = -1;
                return;
            }

            size = (LastWrittenIndex + 1) - length; //TotalLength - length
            memoryStream.Position = length;
            data = binaryReader.ReadBytes(size);
            memoryStream.Position = 0;
            memoryStream.Write(data, 0, size);
            LastWrittenIndex = size - 1;
            data = null;
        }

        private byte[] MainBytes = null;
        private int m_UnCompressMsgLength = 0;
        private readonly int _HeaderLength = 6;
        public ArrayList Defragmentation(byte[] bytData, ref byte[] globalPendingBytes)
        {
            // If message received for defragmentation is null or length is ZERO then reject message
            if (bytData == null || bytData.Length == 0)
            {
                return null;
            }

            if (!(globalPendingBytes == null))
            {
                MainBytes = new byte[globalPendingBytes.Length + bytData.Length];
                Buffer.BlockCopy(globalPendingBytes, 0, MainBytes, 0, globalPendingBytes.Length);
                Buffer.BlockCopy(bytData, 0, MainBytes, globalPendingBytes.Length, bytData.Length);
                bytData = MainBytes;
                globalPendingBytes = null;
            }

            ArrayList FullMessages = new ArrayList();
            bool IsDone = true;
            int MsgLength = 0;

            while (IsDone)
            {
                if (bytData.Length < _HeaderLength)
                {
                    globalPendingBytes = bytData;
                    return FullMessages;
                }

                MsgLength = GetMessageLength(bytData);
                if (MsgLength <= 0)
                {
                    return null;
                }

                if (bytData.Length < (MsgLength + _HeaderLength))
                {
                    globalPendingBytes = bytData;
                    return FullMessages;
                }
                else
                {
                    globalPendingBytes = null;
                }

                byte[] CompressedMessage = new byte[MsgLength];
                Buffer.BlockCopy(bytData, _HeaderLength, CompressedMessage, 0, MsgLength);

                byte[] UnCompressedByteMessage = this.GetUncompressedMessage(CompressedMessage);
                if (UnCompressedByteMessage == null)
                {
                    IsDone = false;
                    break;
                }

                while (true)
                {
                    m_UnCompressMsgLength = 0;
                    m_UnCompressMsgLength = GetMessageLength(UnCompressedByteMessage);

                    if (m_UnCompressMsgLength <= 0)
                    {
                        CompressedMessage = null;
                        UnCompressedByteMessage = null;
                        break;
                    }

                    byte[] unCompressBytes = new Byte[m_UnCompressMsgLength];
                    Buffer.BlockCopy(UnCompressedByteMessage, _HeaderLength, unCompressBytes, 0, m_UnCompressMsgLength);

                    FullMessages.Add(unCompressBytes);

                    byte[] unCompressNewBytes = new Byte[UnCompressedByteMessage.Length - m_UnCompressMsgLength - _HeaderLength];

                    if (unCompressNewBytes.Length <= 0)
                    {
                        CompressedMessage = null;
                        UnCompressedByteMessage = null;
                        break;
                    }

                    Buffer.BlockCopy(UnCompressedByteMessage, m_UnCompressMsgLength + _HeaderLength, unCompressNewBytes, 0, (UnCompressedByteMessage.Length - (m_UnCompressMsgLength + _HeaderLength)));

                    UnCompressedByteMessage = unCompressNewBytes;
                }

                //FullMessages.Add(UnCompressedByteMessage);

                if (bytData.Length == (MsgLength + _HeaderLength))
                {
                    IsDone = false;
                    CompressedMessage = null;
                    break;
                }
                byte[] NewMessage = new byte[bytData.Length - (MsgLength + _HeaderLength)];
                Buffer.BlockCopy(bytData, MsgLength + _HeaderLength, NewMessage, 0, (bytData.Length - (MsgLength + _HeaderLength)));
                bytData = NewMessage;
            }

            MainBytes = null;
            bytData = null;

            return FullMessages;
        }

        private char[] headerChar = new char[5];
        private int GetMessageLength(byte[] message)
        {
            if (message[0] == 5)
            {
                IsUncompress = false;
            }
            else
            {
                IsUncompress = true;
            }

            try
            {
                byte i = 1, startIndex = 0;
                for (; i < _HeaderLength; i++)
                {
                    headerChar[startIndex] = (char)message[i];
                    startIndex++;
                }
                string sLength = new string(headerChar, 0, startIndex);
                int iLength = Convert.ToInt32(sLength);
                return iLength;
            }
            catch
            {
                return 0;
            }
        }

        private byte[] GetUncompressedMessage(byte[] CompressData)
        {
            byte[] uncompressedBytes = null;
            string errString;
            if (!IsUncompress)
            {
                object bytDecomp = null;
                bytDecomp = ZlibCompressor.Uncompress(CompressData, out errString);
                uncompressedBytes = (byte[])bytDecomp;
            }
            else
            {
                uncompressedBytes = CompressData;
            }
            return uncompressedBytes;
        }

        public bool IsUncompress { get; set; }

    }
}
