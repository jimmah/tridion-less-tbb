using System;
using System.Collections.Generic;
using Tridion.ContentManager.Templating;
using dotless.Core.Loggers;

namespace Blocks.Tridion.LessSupport.Compiler
{
    /// <summary>
    /// Wrapper around the Tridion <see cref="TemplatingLogger"/> that implements dotLess's <see cref="ILogger"/> interface.
    /// </summary>
    public class TridionLessLogger : ILogger
    {
        private readonly TemplatingLogger _logger;
        private readonly List<string> _errors;

        public TridionLessLogger(TemplatingLogger logger)
        {
            if (logger == null) throw new ArgumentNullException("logger");

            _logger = logger;
            _errors = new List<string>();
        }

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        public string ErrorMessage
        {
            get { return _errors.Join(Environment.NewLine).Trim(); }
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
            _logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            Info(message.FormatWith(args));
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            Debug(message.FormatWith(args));
        }

        public void Warn(string message)
        {
            _logger.Warning(message);
        }

        public void Warn(string message, params object[] args)
        {
            Warn(message.FormatWith(args));
        }

        public void Error(string message)
        {
            _errors.Add(message);
            _logger.Error(message);
        }

        public void Error(string message, params object[] args)
        {
            message = message.FormatWith(args);
            Error(message);
        }
        // ReSharper restore MethodOverloadWithOptionalParameter
    }
}