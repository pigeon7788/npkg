﻿using CommandLine;
using Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cli
{
    class Program
    {
        [Verb("init", HelpText = "Add file contents to the index.")]
        class InitOptions
        {
            [Option('p', "path", Required = false, Default = null)]
            public string path { set; get; }

            [Option('y', "confirm", Required = false, Default = false)]
            public bool confirm { set; get; }
        }

        [Verb("add", HelpText = "Record changes to the repository.")]
        class AddOptions
        {
            [Value(0, HelpText = "Record changes to the repository.")]
            public string Package { set; get; }

            [Option('g', "global", Required = false, Default = false)]
            public bool Global { set; get; }
        }

        [Verb("remove", HelpText = "Record changes to the repository.")]
        class RemoveOptions
        {
        }

        [Verb("install", HelpText = "Record changes to the repository.")]
        class InstallOptions
        {
        }

        [Verb("publish", HelpText = "Publish package to npkg.net")]
        class PublishOptions
        {
        }

        [Verb("set", HelpText = "Publish package to npkg.net")]
        class SetOptions
        {
            [Value(0, HelpText = "Record changes to the repository.")]
            public string option { set; get; }

            [Value(1, HelpText = "Record changes to the repository.")]
            public string value { set; get; }
        }

        private static PackageManager pm;
        private static string WorkDir { set; get; }

        static int Main(string[] args)
        {

            WorkDir = Directory.GetCurrentDirectory();

            pm = new PackageManager(WorkDir);

            var res = Parser.Default.ParseArguments<InitOptions, AddOptions, PublishOptions, RemoveOptions, InstallOptions, SetOptions>(args)
              .MapResult(
                (InitOptions opts) => InitPkgCommand(opts),
                (AddOptions opts) => AddPkgCommand(opts),
                (PublishOptions opts) => PublishCommand(opts),
                (RemoveOptions opts) => RemoveCommand(opts),
                (InstallOptions opts) => InstallCommand(opts),
                (SetOptions opts) => SetCommand(opts),
                errs => 1);
            return 1;
        }

        private static int InitPkgCommand(InitOptions opts)
        {
            return pm.Init(opts.path, opts.confirm);
        }

        private static int AddPkgCommand(AddOptions opts)
        {
            return pm.Add(opts.Package, opts.Global);
        }

        private static int PublishCommand(PublishOptions opts)
        {

            return pm.Publish();
        }

        private static int RemoveCommand(RemoveOptions opts)
        {
            return 1;
        }

        private static int InstallCommand(InstallOptions opts)
        {
            return 1;
        }

        private static int SetCommand(SetOptions opts)
        {
            return pm.Set(opts.option, opts.value);
        }

        static void log(object str)
        {
            Console.WriteLine(str.ToString());
        }
    }
}
