﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PoshGit2.Utils
{
    public class WindowsCurrentDirectory : ICurrentWorkingDirectory
    {
        private const int MAX_PATH = 260;
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathRelativePathTo([Out] StringBuilder pszPath, [In] string pszFrom, [In] FileAttributes dwAttrFrom, [In] string pszTo, [In] FileAttributes dwAttrTo);

        public string CWD { get { return Environment.CurrentDirectory; } }

        public bool IsValid { get { return true; } }

        public string CreateRelativePath(string path)
        {
            var str = new StringBuilder(MAX_PATH);
            var success = PathRelativePathTo(str, CWD, FileAttributes.Directory, path, FileAttributes.Normal);

            return success ? str.ToString() : string.Empty;
        }
    }
}