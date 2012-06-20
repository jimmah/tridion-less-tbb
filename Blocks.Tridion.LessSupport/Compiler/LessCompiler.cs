using System;
using Blocks.Tridion.LessSupport.IO;
using Tridion.ContentManager.Templating;
using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Parser;

namespace Blocks.Tridion.LessSupport.Compiler
{
    public class LessCompiler
    {
        private readonly TemplatingLogger _logger;

        public LessCompiler(TemplatingLogger logger)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            _logger = logger;
        }

        /// <summary>
        /// Transforms some LESS source code (and any related files) to CSS.
        /// </summary>
        /// <param name="source">The LESS code to transform.</param>
        /// <param name="file">The context file system.</param>
        public string Compile(string source, IFile file)
        {
            var parser = new Parser
            {
                Importer = new Importer(new LessFileReader(file.Directory))
            };

            var lessLogger = new TridionLessLogger(_logger);
            var engine = new LessEngine(parser, lessLogger, false, false);

            var css = engine.TransformToCss(source, string.Empty);

            if (lessLogger.HasErrors)
                throw new LessCompileException(lessLogger.ErrorMessage);

            return css;
        }
    }
}