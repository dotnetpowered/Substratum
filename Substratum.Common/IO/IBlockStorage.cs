using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.IO
{
    public interface IBlockStorage
    {
        byte[] ReadBuffer(long blockNumber);
        void WriteBuffer(long blockNumber, byte[] buffer);
        long BufferSize { get; }
        long Length { get; }
        long Blocks { get; }
    }
}
