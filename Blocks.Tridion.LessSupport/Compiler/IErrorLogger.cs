using dotless.Core.Loggers;

namespace Blocks.Tridion.LessSupport.Compiler
{
    public interface IErrorLogger : ILogger
    {
        bool HasErrors { get; }
        string ErrorMessage { get; }
    }
}