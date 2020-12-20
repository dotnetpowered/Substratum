using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Substratum.IO
{
    public class FileBlockStorage : IBlockStorage
    {
        long bufferSize;
        string path;
        DirectoryInfo dir;

        public FileBlockStorage(DirectoryInfo dir, long bufferSize)
        {
            this.dir = dir;
            this.path = Path.Combine(dir.FullName, "block");
            this.bufferSize = bufferSize;
        }

        public static FileBlockStorage CreateNew(DirectoryInfo parentDir, string name, long bufferSize)
        {
            string newPath = Path.Combine(parentDir.FullName, name);
            if (Directory.Exists(newPath))
                Directory.Delete(newPath, true);
            DirectoryInfo dir = Directory.CreateDirectory(newPath);
            FileBlockStorage s = new FileBlockStorage(dir, bufferSize);
            return s;
        }

        #region IBlockStorage Members

        public byte[] ReadBuffer(long blockNumber)
        {
            return File.ReadAllBytes(path + blockNumber + ".dat");
        }

        public void WriteBuffer(long blockNumber, byte[] buffer)
        {
            File.WriteAllBytes(path + blockNumber + ".dat", buffer);
        }

        public long BufferSize
        {
            get { return bufferSize; }
        }

        public long Length
        {
            get
            {
                long len = 0;
                foreach (FileInfo f in dir.GetFiles())
                {
                    len += f.Length;
                }
                return len;
            }
        }

        public long Blocks
        {
            get
            {
                return dir.GetFiles().Length;
            }
        }

        #endregion
    }

}
