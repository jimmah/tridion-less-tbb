using System;
using Blocks.Tridion.LessSupport.IO;
using dotless.Core.Input;

namespace Blocks.Tridion.LessSupport.Compiler
{
    /// <summary>
    /// Retrieves imported files from the file system for the compiler.
    /// </summary>
    public class LessFileReader : IFileReader
    {
        private readonly IDirectory _directory;

        public LessFileReader(IDirectory directory)
        {
            if (directory == null) throw new ArgumentNullException("directory");

            _directory = directory;
        }

        public byte[] GetBinaryFileContents(string fileName)
        {
            return _directory.FileExists(fileName) ? _directory.GetFile(fileName).BinaryContent : null;
        }

        public string GetFileContents(string fileName)
        {
            return _directory.FileExists(fileName) ? _directory.GetFile(fileName).Content : null;
        }

        public bool DoesFileExist(string fileName)
        {
            return _directory.FileExists(fileName);
        }
    }
}