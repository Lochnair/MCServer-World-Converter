/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2015 Nils Andreas Svee
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MCServer_World_Converter
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Load assemblies
            EmbeddedAssembly.Load("MCServer_World_Converter.Newtonsoft.Json.dll", "Newtonsoft.Json.dll");
            EmbeddedAssembly.Load("MCServer_World_Converter.Substrate.dll", "Substrate.dll");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            // Dirty hack for switching between console and GUI
            if (!IsRunningOnMono() && args.Length > 0)
            {
                AttachConsole(-1);
            }

            if (args.Length == 2)
            {
                if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine("Error: Source directory doesn't exist!");
                    return;
                }

                if (!Directory.Exists(args[1]))
                {
                    Console.WriteLine("Error: Output directory doesn't exist!");
                    return;
                }

                MainForm.Run(args[0], args[1], true);
            }
            else if (args.Length > 0)
            {
                Console.WriteLine("Usage: MCServer.World.Converter <sourceDirectory> <outputDirectory>");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }

        static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
