namespace Blocks.Tridion.LessSupport.IO
{
    /// <summary>
    /// Interface to represent an in-memory file.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// The containing <see cref="IDirectory"/>.
        /// </summary>
        IDirectory Directory { get; set; }

        /// <summary>
        /// The file name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The raw binary data of the file.
        /// </summary>
        byte[] BinaryContent { get; set; }

        /// <summary>
        /// A UTF-8 encoded string representation of the <see cref="BinaryContent"/>.
        /// </summary>
        string Content { get; set; }
    }
}