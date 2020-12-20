using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace Substratum.IO
{
    public enum StorageDirection
    {
        None,
        Read,
        Write
    }
    
    public class BlockStream : Stream
    {
        long length = 0;
        byte[] streamBuffer;
        long bufferIndex = 0;
        long position = 0;
        long blockIndex = 0;
        long lastFlushPosition = 0;
        IBlockStorage storage;
        StorageDirection storageDirection;
        bool eof;

        public BlockStream(IBlockStorage storage, StorageDirection storageDirection)
        {
            this.storage = storage;
            this.storageDirection = storageDirection;
        }

        public override void Close()
        {
            Flush();
            storageDirection = StorageDirection.None;
            base.Close();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        // ===============================================================================
        // Seek Support
        // ===============================================================================

        public override bool CanSeek
        {
            get { return (storageDirection==StorageDirection.Read); }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!CanSeek)
                throw new NotSupportedException();

            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = offset;
                    break;
                case SeekOrigin.Current:
                    position+=offset;
                    break;
                case SeekOrigin.End:
                    position-=offset;
                    break;
            }
            blockIndex = position / storage.BufferSize;
            streamBuffer = storage.ReadBuffer(blockIndex);
            bufferIndex = position % storage.BufferSize;
            return position;
        }

        // ===============================================================================
        // Read Support
        // ===============================================================================

        public override bool CanRead
        {
            get { return (storageDirection == StorageDirection.Read); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int ReadCount = 0;
            for (int i = offset; i < offset + count && i < buffer.Length && !eof; i++)
            {
                buffer[i] = (byte) ReadByte();
                if (!eof)
                    ReadCount++;
            }
            return ReadCount;
        }

        public override int ReadByte()
        {
            if (!CanRead)
                throw new NotSupportedException();

            if (eof)
                return -1;

            if (streamBuffer==null)
                streamBuffer = storage.ReadBuffer(blockIndex);

            if (bufferIndex == streamBuffer.Length)
            {
                blockIndex++;
                if (blockIndex == storage.Blocks)
                {
                    eof = true;
                    return -1;
                }
                streamBuffer = storage.ReadBuffer(blockIndex);
                bufferIndex = 0;
            }
            byte ReadByte = streamBuffer[bufferIndex];
            bufferIndex++;
            position++;
            return ReadByte;
        }

        // ===============================================================================
        // Write Support
        // ===============================================================================

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                Seek(position, SeekOrigin.Begin);
            }
        }

        public override long Length
        {
            get { return length; }
        }

        public override void Flush()
        {
            if (lastFlushPosition == position)
                return;

            if (bufferIndex == streamBuffer.Length)
                storage.WriteBuffer(blockIndex, streamBuffer);
            else
            {
                byte[] tempBuffer=new byte[bufferIndex];
                Buffer.BlockCopy(streamBuffer, 0, tempBuffer, 0, (int) bufferIndex);
                storage.WriteBuffer(blockIndex, tempBuffer);
            }
            lastFlushPosition = position;
        }

        public override bool CanWrite
        {
            get { return (storageDirection == StorageDirection.Write); }
        }

        public override void WriteByte(byte value)
        {
            if (streamBuffer == null)
                this.streamBuffer = new byte[storage.BufferSize];

            if (bufferIndex == streamBuffer.Length)
            {
                Flush();
                bufferIndex = 0;
                blockIndex++;
                streamBuffer = new byte[storage.BufferSize];
            }
            streamBuffer[bufferIndex] = value;
            bufferIndex++;
            length++;
            position++;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite)
                throw new NotSupportedException();

            for (int i=offset;i<offset+count;i++)
            {
                WriteByte(buffer[i]);
            }
        }
    }
}
