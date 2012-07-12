﻿/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;

namespace CustomToolkit
{
    /// <summary>
    /// Creates HTML from Markdown deriving the output file extension from the input filename
    /// or using the FileNameSpace property if no inline extension is provided
    /// </summary>
    [ComVisible(true)]
    [Guid("a6c15c51-199f-4f07-aba5-3c0827e951a3")]
    //have to register for every language project or else it might not work
    [CodeGeneratorRegistration(typeof(Markdown), "Markdown For VS", vsContextGuids.vsContextGuidVCSProject,
        GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(Markdown), "Markdown For VS", vsContextGuids.vsContextGuidVBProject,
        GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(Markdown), "Markdown For VS", vsContextGuids.vsContextGuidVJSProject,
        GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(Markdown))]
    public class Markdown : CustomToolkit.BaseCodeGeneratorWithSite
    {
#pragma warning disable 0414
        //The name of this generator (use for 'Custom Tool' property of project item).  from steve:  no idea why this is necessary.  but it breaks if you comment this out.  and if you change this, changing the code generator name in VS doesn't work.  It's like this and the class name need to be the same.
        internal static string name = "Markdown";
#pragma warning restore 0414

        private string DefaultExtension = string.Empty;
        /// <summary>
        /// Returns the extension for the generated file
        /// </summary>
        /// <returns></returns>
        protected override string GetDefaultExtension()
        {
            return DefaultExtension;
        }

        /// <summary>
        /// Function that builds the contents of the generated file based on the contents of the input file
        /// </summary>
        /// <param name="inputFileContent">Content of the input file</param>
        /// <returns>Generated file as a byte array</returns>
        protected override byte[] GenerateCode(string inputFileContent)
        {
            try
            {               
                //If the input file doesn't contain an inline output extension then use the file namespace,
                if (new Regex(@"(\.\w+)\" + Path.GetExtension(InputFilePath))
                    .Match(Path.GetFileName(InputFilePath)).Groups.Count == 1)
                {
                    DefaultExtension = "." + this.FileNameSpace;
                }
                else //leave the default extension blank to use the inline extension
                {
                    DefaultExtension = string.Empty;
                }

                var input = File.ReadAllText(InputFilePath);
                var md = new MarkdownSharp.Markdown();
                var output = md.Transform(input);
                return ConvertToBytes(output);
            }
            catch (Exception exception)
            {
                GeneratorError(0, exception.Message, 0, 0);
            }

            return null;
        }

        /// <summary>
        /// Takes the file contents and converts them to a byte[] that VS can use to update generated code.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(string content)
        {
            //Get the preamble (byte-order mark) for our encoding
            byte[] preamble = Encoding.UTF8.GetPreamble();
            int preambleLength = preamble.Length;

            byte[] body = Encoding.UTF8.GetBytes(content);

            //Prepend the preamble to body (store result in resized preamble array)
            Array.Resize(ref preamble, preambleLength + body.Length);
            Array.Copy(body, 0, preamble, preambleLength, body.Length);

            //Return the combined byte array
            return preamble;
        }

    }
}