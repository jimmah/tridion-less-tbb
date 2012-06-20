using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Blocks.Tridion.LessSupport.Compiler;
using Blocks.Tridion.LessSupport.IO;
using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.Templating;
using Tridion.ContentManager.Templating.Assembly;

namespace Blocks.Tridion.LessSupport
{
    /// <summary>
    /// A simple Template Building Block that produces CSS from a LESS file.
    /// </summary>
    [TcmTemplateTitle("Process LESS")]
    public class ProcessLess : ITemplate
    {
        private readonly TemplatingLogger _log;
        private readonly TridionLessLogger _lessLog;
        private Engine _engine;
        private Package _package;

        public ProcessLess()
        {
            _log = TemplatingLogger.GetLogger(typeof(ProcessLess));
            _lessLog = new TridionLessLogger(_log);
        }

        /// <summary>
        /// The current <see cref="Component"/>.
        /// </summary>
        private Component Component
        {
            get
            {
                var component = _package.GetByType(ContentType.Component);
                return (component != null) ? (Component) _engine.GetObject(component.GetAsSource().GetValue("ID")) : null; 
            }
        }

        /// <summary>
        /// The WebDAV path for the current <see cref="Component"/>.
        /// </summary>
        private string ComponentWebDAVPath
        {
            get
            {
                var cmp = Component;
                return cmp != null ? cmp.OrganizationalItem.WebDavUrl : null;
            }
        }

        /// <summary>
        /// The current <see cref="ComponentTemplate"/>.
        /// </summary>
        private ComponentTemplate ComponentTemplate
        {
            get
            {
                var template = _engine.PublishingContext.ResolvedItem.Template;
                return template as ComponentTemplate;
            }
        }

        public void Transform(Engine engine, Package package)
        {
            if (engine == null) throw new ArgumentNullException("engine");
            if (package == null) throw new ArgumentNullException("package");

            _engine = engine;
            _package = package;

            // Get the raw LESS
            var less = string.Empty;
            var raw = Component.BinaryContent.GetByteArray();
            if (raw != null)
                less = Encoding.UTF8.GetString(raw, 0, raw.Length);

            // The "virtual" file system allows the LESS compiler to locate @import-ed files.
            var file = new VirtualFile(Component.Title) {BinaryContent = raw};
            var dir = new VirtualDirectory();
            dir.AddFile(file);

            // Process any @imports
            ProcessImports(less, file);

            // Transform to CSS
            var css = CompileLess(less, file);

            // Process any url(..) images
            css = ProcessImages(css);
            
            // Add to package
            _package.PushItem("Output", package.CreateStringItem(ContentType.Text, css));
        }

        /// <summary>
        /// Passes the LESS source (and any @import-ed files) to the LESS Compiler and
        /// transforms it to CSS.
        /// </summary>
        private string CompileLess(string source, IFile file)
        {
            try
            {
                var compiler = new LessCompiler(_lessLog);
                var css = compiler.Compile(source, file);

                return css;
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred in ProcessLess.CompileLess: {0}".FormatWith(ex.Message), ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Looks for any @import statements in the LESS file being parsed and
        /// retrieves the content of these files.
        /// </summary>
        /// <remarks>
        /// Any imported LESS files must be in the same folder in Tridion.
        /// </remarks>
        private void ProcessImports(string source, IFile file)
        {
            try
            {
                var webdav = ComponentWebDAVPath;

                // ReSharper disable LoopCanBePartlyConvertedToQuery
                foreach (Match match in Regex.Matches(source, @"@import(?:\s)(?:"")?([a-zA-z.\-_]*)"))
                {
                    var fileName = match.Groups[1].Value;
                    var path = "{0}/{1}{2}".FormatWith(webdav, fileName, fileName.GetExtension(".less"));

                    try
                    {
                        var import = (Component) _engine.GetObject(path);

                        if (import == null)
                        {
                            throw new Exception("Unable to locate component at {0}".FormatWith(path));
                        }

                        var importedFile = new VirtualFile(fileName)
                        {
                            BinaryContent = import.BinaryContent.GetByteArray()
                        };

                        file.Directory.AddFile(importedFile);
                    }
                    catch (Exception ex)
                    {
                        _log.Warning("Failed to locate import {0}: {1}".FormatWith(fileName, ex.Message));
                    }
                }
                // ReSharper restore LoopCanBePartlyConvertedToQuery
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred in ProcessLess.ProcessImports: {0}".FormatWith(ex.Message), ex);
            }
        }

        private string ProcessImages(string source)
        {
            try
            {
                var webdav = ComponentWebDAVPath;
                var failures = new List<string>();
                
                var images = Regex.Matches(source, @"url\((\S*)\)");
                foreach (Match match in images)
                {
                    var image = match.Groups[1].Value;

                    var imagePath = "{0}/{1}".FormatWith(webdav, image.Replace("../", ""));

                    try
                    {
                        var img = (Component) _engine.GetObject(imagePath);

                        if (img == null)
                        {
                            throw new Exception("Unable to locate component at {0}".FormatWith(imagePath));
                        }

                        var publishPath = _engine.AddBinary(img.Id, ComponentTemplate.Id, null,
                                                            img.BinaryContent.GetByteArray(),
                                                            img.Title + image.GetExtension(".gif"));

                        source = source.Replace(image, publishPath);
                    }
                    catch (Exception ex)
                    {
                        failures.Add(image);
                        _log.Warning("Failed to locate image {0}: {1}".FormatWith(image, ex.Message));
                    }
                }

                source = AppendFailures(source, "Images Not Found", failures);
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred in ProcessLess.ProcessImage: {0}".FormatWith(ex.Message), ex);
            }

            return source;
        }

        private static string AppendFailures(string source, string title, IEnumerable<string> failures)
        {
            source += "{0}/*** {1} ***/{0}".FormatWith(Environment.NewLine, title);
            source += "/* {0} */".FormatWith(failures.Join(Environment.NewLine));

            return source;
        }
    }
}