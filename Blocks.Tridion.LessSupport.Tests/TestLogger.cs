using System;
using System.Collections.Generic;
using Blocks.Tridion.LessSupport.Compiler;
using dotless.Core.Loggers;

namespace Blocks.Tridion.LessSupport.Tests
{
    public class TestLogger : IErrorLogger 
    {
        private readonly List<string> _errors;

        public TestLogger()
        {
            _errors = new List<string>();
        }

        public void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Info:
                    Info(message);
                    break;
                case LogLevel.Debug:
                    Debug(message);
                    break;
                case LogLevel.Warn:
                    Warn(message);
                    break;
                case LogLevel.Error:
                    Error(message);
                    break;
            }
        }

        // ReSharper disable MethodOverloadWithOptionalParameter
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Error(string message)
        {
            _errors.Add(message);
            Console.WriteLine(message);
        }

        public void Error(string message, params object[] args)
        {
            message = message.FormatWith(args);
            Error(message);
        }
        // ReSharper restore MethodOverloadWithOptionalParameter

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        public string ErrorMessage
        {
            get { return _errors.Join(Environment.NewLine).Trim(); }
        }
    }
}