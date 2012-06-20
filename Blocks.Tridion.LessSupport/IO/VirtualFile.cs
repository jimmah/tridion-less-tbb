using System.Text;

namespace Blocks.Tridion.LessSupport.IO
{
    public class VirtualFile : IFile
    {
        private readonly string _fileName;

        /// <summary>
        /// The file name.
        /// </summary>
        public string Name
        {
            get { return _fileName; }
        }

        /// <summary>
        /// The containing <see cref="IDirectory"/>.
        /// </summary>
        public IDirectory Directory { get; set; }

        /// <summary>
        /// A UTF-8 encoded string representation of the <see cref="IFile.BinaryContent"/>.
        /// </summary>
        public string Content
        {
            get { return Encoding.UTF8.GetString(BinaryContent); }
            set { BinaryContent = Encoding.UTF8.GetBytes(value); }
        }

        /// <summary>
        /// The raw binary data of the file.
        /// </summary>
        public byte[] BinaryContent { get; set; }

        public VirtualFile(string fileName)
        {
            _fileName = fileName;
        }

        public bool Equals(VirtualFile other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Equals(other._fileName, _fileName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (VirtualFile) && Equals((VirtualFile) obj);
        }

        public override int GetHashCode()
        {
            return (_fileName != null ? _fileName.GetHashCode() : 0);
        }
    }
}