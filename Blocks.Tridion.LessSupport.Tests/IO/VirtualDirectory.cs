using Blocks.Tridion.LessSupport.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Blocks.Tridion.LessSupport.Tests.IO
{
    [TestFixture, Category("IO")]
    public class VirtualDirectoryTests
    {
        [Test]
        public void Can_add_and_retrieve_files()
        {
            var dir = new VirtualDirectory();
            var file = new VirtualFile("testfile.txt");

            dir.AddFile(file);

            var retrieved = dir.GetFile("testfile.txt");

            retrieved.Should().NotBeNull().And.BeSameAs(file);
        }

        [Test]
        public void Should_be_able_to_check_if_file_exists()
        {
            var dir = new VirtualDirectory();
            var file = new VirtualFile("testfile.txt");

            dir.AddFile(file);

            dir.FileExists("testfile.txt").Should().BeTrue();
        }
    }
}