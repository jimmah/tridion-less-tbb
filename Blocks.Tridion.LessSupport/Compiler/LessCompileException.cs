using System;

namespace Blocks.Tridion.LessSupport.Compiler
{
    public class LessCompileException : Exception
    {
        public LessCompileException(string message, Exception inner)
            : base(message, inner) {}

        public LessCompileException(string message)
            : base(message) {}
    }
}