using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Substratum.IO
{
    public enum ProcessingDirection
    {
        Read,
        Write
    }

    public abstract class ProcessingStream : Stream
    {
        private Stream stream;

        public ProcessingStream(Stream stream)
        {
            this.stream = stream;
        }

        public override bool CanRead
        {
            get { return stream.CanRead; }
        }

        public override bool CanWrite
        {
            get { return stream.CanWrite; }
        }

        public override bool CanSeek
        {
            get { return stream.CanSeek; }
        }

        public override long Length
        {
            get { throw new NotSupportedException("Property not supported."); }
        }

        public override long Position
        {
            get { throw new NotSupportedException("Property not supported."); }
            set { throw new NotSupportedException("Property not supported."); }
        }

        public override void Flush()
        {
            stream.Flush();
        }

        public override void Close()
        {
            stream.Close();
            base.Close();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
            //throw new NotSupportedException("The method or operation is not supported.");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Method not supported.");
        }

        public override int Read(byte[] buffer, int offset, int count) 
        {
            byte[] tempBuffer = new byte[count];
            int numRead = stream.Read(tempBuffer, 0, count);
            byte[] outBuffer;
            int outLen;

            ProcessBuffer(ProcessingDirection.Read, tempBuffer, 0, numRead, out outBuffer, out outLen);

            for (int i = 0; i < outLen; i++)
                buffer[i+offset] = tempBuffer[i];

            return outLen;
        }

        public override void Write(byte[] buffer, int offset, int count) 
        {
            byte[] outBuffer;
            int outLen;

            ProcessBuffer(ProcessingDirection.Write, buffer, offset, count, out outBuffer, out outLen);

            stream.Write(outBuffer, 0, outLen);
        }

        protected abstract void ProcessBuffer(ProcessingDirection Direction, byte[] buffer, int offset, int count,
            out byte[] outBuffer, out int outCount);

    } 
}
