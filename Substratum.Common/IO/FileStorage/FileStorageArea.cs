using System;
using System.IO;
using Substratum.Common;
using Substratum.FileStorage.StorageHandlers;

namespace Substratum.FileStorage
{
    public class FileStorageArea
    {
        IFileStorageHandler handler;

        public FileStorageArea(IFileStorageHandler handler)
        {
            this.handler = handler;
        }

        public FileReference Store(FileReference fileRef)
        {
            return handler.Store(this, fileRef);
        }

        public FileReference Store(string Name, Stream Stream)
        {
            string ContentType = MimeTypes.GetMimeType(Name);
            return this.Store(Name, Stream, ContentType);
        }

        public FileReference Store(string Name, Stream Stream, string ContentType)
        {
            var fileRef = handler.Store(this, Name, ContentType, Stream);
            return fileRef;
        }

        public Stream GetContentStream(string FileHandle)
        {
            return handler.GetContentStream(this, FileHandle);
        }

        public FileReference GetFile(string FileHandle)
        {
            return handler.GetFile(this, FileHandle);
        }

        public static FileStorageArea GetTempFileStorageArea()
        {
            string TempPath = Environment.GetEnvironmentVariable("TEMP");
            return new FileStorageArea(new CIFSFileStorageHandler(TempPath));
        }
    }
}
