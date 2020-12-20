using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Drawing;

namespace Substratum.FileStorage.StorageHandlers
{
    public class CIFSFileStorageHandler : IFileStorageHandler
    {
        int BytesToRead = 1024 * 4;
        string BasePath;

        public CIFSFileStorageHandler(string BasePath)
        {
            this.BasePath = BasePath;
        }

        #region IFileStorageHandler Members

        public FileReference Store(FileStorageArea StorageArea, string Name, string ContentType, 
            System.IO.Stream Stream)
        {
            Guid fileHandle;
            long Size;
            StoreFile(Name, ContentType, Stream, out fileHandle, out Size);

            return FileReference.FromNewFile(StorageArea, Name, Size, fileHandle.ToString(), ContentType);
        }

        private void StoreFile(string Name, string ContentType, System.IO.Stream Stream, out Guid fileHandle, out long Size)
        {
            if (!Directory.Exists(BasePath))
                Directory.CreateDirectory(BasePath);

            fileHandle = Guid.NewGuid();
            var fileDir = Directory.CreateDirectory(Path.Combine(BasePath, fileHandle.ToString()));
            string fullPath = Path.Combine(fileDir.FullName, Name);
            Size = 0;
            int bytesRead;
            using (var fs = File.Create(fullPath))
            {
                BinaryReader reader = new BinaryReader(Stream);
                byte[] buffer = new byte[BytesToRead];
                do
                {
                    bytesRead = reader.Read(buffer, 0, BytesToRead);
                    fs.Write(buffer, 0, bytesRead);
                    Size += bytesRead;
                }
                while (bytesRead > 0);
            }
            XElement metaData = new XElement("metadata",
                new XElement("contentType", ContentType)
                );
            metaData.Save(Path.Combine(fileDir.FullName, "_metadata.xml"));
        }

        public Stream GetContentStream(FileStorageArea StorageArea, string FileHandle)
        {
            var fileDir = new DirectoryInfo(Path.Combine(BasePath, FileHandle.ToString()));
            foreach (var fileInfo in fileDir.GetFiles())
            {
                if (fileInfo.Name != "_metadata.xml" && !fileInfo.Name.StartsWith("_thumbnail"))
                    return fileInfo.OpenRead();
            }
            return null;
        }

        public FileReference GetFile(FileStorageArea StorageArea, string FileHandle)
        {
            var fileDir = new DirectoryInfo(Path.Combine(BasePath, FileHandle.ToString()));
            string contentType = string.Empty;
            string filename = null;
            long fileLength = 0;
            bool thumbnailFound = false;
            foreach (var fileInfo in fileDir.GetFiles())
            {
                if (fileInfo.Name == "_metadata.xml")
                {
                    contentType = XElement.Load(fileInfo.FullName).Element("contentType").Value;
                }
                else if (!fileInfo.Name.StartsWith("_thumbnail"))
                {
                    filename = fileInfo.Name;
                    fileLength = fileInfo.Length;
                }
                else
                    thumbnailFound = true;
            }
            return new FileReference(StorageArea, filename, fileLength, FileHandle, contentType) { HasThumbnail = thumbnailFound }; 
        }

        public FileReference Store(FileStorageArea fileStorageArea, FileReference fileRef)
        {
            using (var stream = fileRef.GetContentStream())
            {
                Guid fileHandle;
                long Size;
                StoreFile(fileRef.Name, fileRef.ContentType, stream, out fileHandle, out Size);

                var fileRef2 = new FileReference(fileStorageArea, fileRef.Name, Size, fileHandle.ToString(), fileRef.ContentType);
                fileRef2.HasThumbnail = fileRef.HasThumbnail;

                return fileRef2;
            }
        }

        #endregion
    }
}
