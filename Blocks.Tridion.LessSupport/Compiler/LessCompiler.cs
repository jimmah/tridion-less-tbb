using System;
using System.IO;
using Blocks.Tridion.LessSupport.IO;
using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Parser;

namespace Blocks.Tridion.LessSupport.Compiler
{
    public class LessCompiler
    {
        private readonly IErrorLogger _logger;

        public LessCompiler(IErrorLogger logger)
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
            var parser = new Parser {Importer = new Importer(new LessFileReader(file.Directory))};

            var engine = new LessEngine(parser, _logger, false, false);

            string css = null;

            try
            {
                css = engine.TransformToCss(source, file.Name);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error("The file you are trying to import '{0}' cannot be found.".FormatWith(ex.FileName));
            }

            if (_logger.HasErrors)
                throw new LessCompileException(_logger.ErrorMessage);

            return css;
        }
    }
}