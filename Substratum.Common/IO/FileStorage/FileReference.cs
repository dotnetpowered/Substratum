using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Substratum.FileStorage
{
    public class FileReference
    {
        public FileReference(FileStorageArea StorageArea, string Name, long Size, string FileHandle, string ContentType)
        {
            this.StorageArea = StorageArea;
            this.Name = Name;
            this.Size = Size;
            this.FileHandle = FileHandle;
            this.ContentType = ContentType;
            this.HasThumbnail = HasThumbnail;
        }

        public string Name { get; private set; }
        public long Size { get; private set; }
        public FileStorageArea StorageArea { get; private set; }
        public string FileHandle { get; private set; }
        public string ContentType { get; private set; }
        public bool HasThumbnail { get; set; }

        public Stream GetContentStream()
        {
            return StorageArea.GetContentStream(this.FileHandle);
        }

        public static FileReference FromNewFile(FileStorageArea StorageArea, string Name, long Size, string FileHandle, 
            string ContentType)
        {
            FileReference fileRef = new FileReference(StorageArea, Name, Size, FileHandle, ContentType);

            return fileRef;
        }
    }
}
