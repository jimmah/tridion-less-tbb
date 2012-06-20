using Blocks.Tridion.LessSupport.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Blocks.Tridion.LessSupport.Tests.IO
{
    [TestFixture, Category("IO")]
    public class VirtualFileTests
    {
        [Test]
        public void Setting_content_sets_binary_content()
        {
            var file = new VirtualFile("testfile.txt") {Content = "blah blah blah"};
            file.BinaryContent.Should().NotBeNull();
        }
    }
}