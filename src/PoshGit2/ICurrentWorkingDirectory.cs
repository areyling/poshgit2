﻿namespace PoshGit2
{
    public interface ICurrentWorkingDirectory
    {
        bool IsValid { get; }
        string CWD { get; }
        string CreateRelativePath(string path);
    }
}