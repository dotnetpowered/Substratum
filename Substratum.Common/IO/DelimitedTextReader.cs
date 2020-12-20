using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Substratum.IO
{
    public class DelimitedTextReader : TextReader
    {
        TextReader reader;
        int bufferSize=100;
        char[] currentBuffer;
        string currrentDelimiter="\r\n";

        public DelimitedTextReader(TextReader reader)
            : base()
        {
            this.reader = reader;
        }

        public DelimitedTextReader(TextReader reader, string delimiter) : base()
        {
            this.currrentDelimiter = delimiter;
            this.reader = reader;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            StringBuilder builder = new StringBuilder();
            ReadLine(builder, count);
            builder.CopyTo(0, buffer, index, builder.Length);
            return builder.Length;
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            StringBuilder builder = new StringBuilder();
            ReadLine(builder, count);
            builder.CopyTo(0, buffer, index, builder.Length);
            return builder.Length;
        }

        public bool EndOfStream
        {
            get
            {
                if (currentBuffer == null)
                    ReadBuffer();
                return currentBuffer.Length == 0;
            }
        }

        public string Delimiter
        {
            set
            {
                currrentDelimiter = value;
            }
            get
            {
                return currrentDelimiter;
            }
        }

        public int BufferSize
        {
            set
            {
                bufferSize = value;
            }
            get
            {
                return bufferSize;
            }
        }

        public virtual void ReadLine(StringBuilder builder, int length)
        {
            if (currentBuffer == null)
                ReadBuffer();
            int i = length;
            while (!EndOfStream && builder.Length < length && i >= currentBuffer.Length)
            {
                if (currentBuffer.Length <= length)
                {
                    builder.Append(currentBuffer);
                    i -= currentBuffer.Length;
                    ReadBuffer();
                }
            }
            ProcessPartialBuffer(i, builder, 0);
        }

        public virtual string ReadLine(int length)
        {
            StringBuilder builder = new StringBuilder();
            ReadLine(builder, length);
            return builder.ToString();            
        }

        public override string ReadToEnd()
        {
            StringBuilder builder=new StringBuilder();
            while (!EndOfStream)
            {
                builder.Append(currentBuffer);
                ReadBuffer();
            }
            return builder.ToString();
        }

        public override int Read()
        {
            if (EndOfStream)
                return -1;
            else
            {
                char c = ReadLine(1)[0];
                return (int)c;
            }
        }

        public override int Peek()
        {
            if (EndOfStream)
                return -1;
            else
                return (int)currentBuffer[0];
        }

        public override void Close()
        {
            reader.Close();
            base.Close();
        }

        public override string ReadLine()
        {
            return ReadLine(currrentDelimiter);
        }


        public virtual void ReadLine(StringBuilder builder, string delimiter)
        {
            if (currentBuffer == null)
                ReadBuffer();
            bool delimeterFound = false;

            int i = 0;
            while (!delimeterFound && !EndOfStream)
            {
                if (currentBuffer[i] == delimiter[0])
                {
                    delimeterFound = true;
                }
                else
                {
                    i++;
                    if (i == currentBuffer.Length)
                    {
                        builder.Append(currentBuffer);
                        ReadBuffer();
                        i = 0;
                    }
                }
            }
            ProcessPartialBuffer(i, builder, delimiter.Length);
        }

        public virtual string ReadLine(string delimiter)
        {
            StringBuilder builder = new StringBuilder();
            ReadLine(builder, delimiter);
            return builder.ToString();
        }

        private void ProcessPartialBuffer(int i, StringBuilder builder, int delimiterSize)
        {
            char[] partialBuffer;
            if (i != 0)
            {
                partialBuffer = new char[i];
                Buffer.BlockCopy(currentBuffer, 0, partialBuffer, 0, sizeof(char) * i);
                builder.Append(partialBuffer);
            }
            if (currentBuffer.Length - i > delimiterSize)
            {
                partialBuffer = new char[currentBuffer.Length - i - delimiterSize];
                Buffer.BlockCopy(currentBuffer, sizeof(char) * (i + delimiterSize), partialBuffer, 0, sizeof(char) * (currentBuffer.Length - i - delimiterSize));
                currentBuffer = partialBuffer;
            }
            else
                currentBuffer = null;
        }

        private void ReadBuffer()
        {
            char[] buffer = new char[bufferSize];
            int readCnt = reader.ReadBlock(buffer, 0, buffer.Length);
            if (buffer.Length != readCnt)
            {
                char[] sizedBuffer = new char[readCnt];
                Buffer.BlockCopy(buffer, 0, sizedBuffer, 0, readCnt*sizeof(char));
                currentBuffer = sizedBuffer;
            }
            else
                currentBuffer = buffer;
        }
    }
}
