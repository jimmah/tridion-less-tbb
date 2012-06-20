namespace Blocks.Tridion.LessSupport.IO
{
    /// <summary>
    /// Interface to represent an in-memory directory.
    /// </summary>
    public interface IDirectory
    {
        /// <summary>
        /// Adds a <see cref="IFile"/> to the current <see cref="IDirectory"/>.
        /// </summary>
        void AddFile(IFile file);

        /// <summary>
        /// Retrieves an <see cref="IFile"/> by it's name from the current <see cref="IDirectory"/>.
        /// </summary>
        IFile GetFile(string fileName);

        /// <summary>
        /// Determines whether a given <see cref="IFile"/> is present in the current <see cref="IDirectory"/>.
        /// </summary>
        bool FileExists(string fileName);
    }
}