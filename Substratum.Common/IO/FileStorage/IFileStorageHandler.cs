using System.IO;

namespace Substratum.FileStorage
{
    public interface IFileStorageHandler
    {
        FileReference Store(FileStorageArea StorageArea, string Name, string ContentType, Stream Stream);
        Stream GetContentStream(FileStorageArea StorageArea, string FileHandle);
        FileReference GetFile(FileStorageArea StorageArea, string Name);
        FileReference Store(FileStorageArea fileStorageArea, FileReference fileRef);
    }
}
