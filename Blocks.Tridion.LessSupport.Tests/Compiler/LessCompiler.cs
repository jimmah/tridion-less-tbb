using Blocks.Tridion.LessSupport.Compiler;
using Blocks.Tridion.LessSupport.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Blocks.Tridion.LessSupport.Tests.Compiler
{
    [TestFixture, Category("LESS")]
    public class LessCompilerTests
    {
        private LessCompiler _compiler;
        private VirtualFile _file;

        [SetUp]
        public void Setup()
        {
            var dir = new VirtualDirectory();
            _file = new VirtualFile("test.less") {Directory = dir};
            _compiler = new LessCompiler(new TestLogger());
        }

        [Test]
        public void Compile_converts_valid_LESS_to_CSS()
        {
            var css = _compiler.Compile("@color: #4d926f; #header { color: @color; }", _file);
            css.Should().Be("#header {\n  color: #4d926f;\n}\n");
        }

        [Test]
        public void Argb_function_should_be_valid()
        {
            var css = _compiler.Compile("#header { color: argb(#123456); }", _file);
            css.Should().Be("#header {\n  color: #ff123456;\n}\n");
        }

        [Test]
        public void Compile_throws_exception_with_invalid_LESS()
        {
            var exception = Assert.Throws<LessCompileException>(() => _compiler.Compile("#unclosed_rule {", _file));
            exception.Message.Should().StartWith("Missing closing '}' on line 1 in file 'test.less':");
        }

        [Test]
        public void Compile_throws_exception_with_unknown_variable()
        {
            var exception = Assert.Throws<LessCompileException>(() => _compiler.Compile("form { \nmargin-bottom: @baseline; }", _file));
            exception.Message.Should().StartWith("variable @baseline is undefined on line 2 in file 'test.less':");
        }

        [Test]
        public void Compile_throws_exception_with_unknown_mixin()
        {
            var exception = Assert.Throws<LessCompileException>(() => _compiler.Compile("form { \n.mymixin; }", _file));
            exception.Message.Should().StartWith(".mymixin is undefined on line 2 in file 'test.less':");
        }

        [Test]
        public void Can_compile_LESS_that_imports_another_LESS_file()
        {
            StubFile("other.less", "@color: #fff;");

            var css = _compiler.Compile("@import \"other.less\";\nbody { color: @color }", _file);
            css.Should().Be("body {\n  color: white;\n}\n");
        }

        [Test]
        public void Compile_throws_exception_when_importing_unknown_file()
        {
            var exception = Assert.Throws<LessCompileException>(
                () => _compiler.Compile("@import \"MISSING.less\";\nbody { color: @color; }", _file));
            exception.Message.Should().StartWith("The file you are trying to import 'MISSING.less' cannot be found.");
        }

        [Test]
        public void Compile_throws_exception_when_using_mixin_from_imported_CSS_file()
        {
            StubFile("other.css", ".mymixin { color: red; }");

            var exception = Assert.Throws<LessCompileException>(
                () => _compiler.Compile("@import \"other.css\";\nbody { .mymixin }", _file));
            exception.Message.Should().StartWith(".mymixin is undefined on line 2 in file 'test.less':");
        }

        private void StubFile(string fileName, string content)
        {
            var file = new VirtualFile(fileName) {Content = content};
            _file.Directory.AddFile(file);
        }
    }
}