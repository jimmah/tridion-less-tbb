using System.Collections.Generic;
using System.Linq;

namespace Blocks.Tridion.LessSupport.IO
{
    public class VirtualDirectory : IDirectory
    {
        private readonly IList<IFile> _files;

        public VirtualDirectory()
        {
            _files = new List<IFile>();
        }

        /// <summary>
        /// Adds a <see cref="IFile"/> to the current <see cref="IDirectory"/>.
        /// </summary>
        public void AddFile(IFile file)
        {
            if (_files.Contains(file)) return;

            file.Directory = this;
            _files.Add(file);
        }

        /// <summary>
        /// Retrieves an <see cref="IFile"/> by it's name from the current <see cref="IDirectory"/>.
        /// </summary>
        public IFile GetFile(string fileName)
        {
            return _files.SingleOrDefault(f => f.Name == fileName);
        }

        /// <summary>
        /// Determines whether a given <see cref="IFile"/> is present in the current <see cref="IDirectory"/>.
        /// </summary>
        public bool FileExists(string fileName)
        {
            return GetFile(fileName) != null;
        }

        public bool Equals(VirtualDirectory other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Equals(other._files, _files);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (VirtualDirectory) && Equals((VirtualDirectory) obj);
        }

        public override int GetHashCode()
        {
            return (_files != null ? _files.GetHashCode() : 0);
        }
    }
}