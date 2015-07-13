﻿using System.Linq;
using System.Threading.Tasks;

namespace PoshGit2.Writers
{
    public abstract class StatusWriter : IStatusWriter
    {
        private readonly IGitPromptSettings _settings;
        private readonly Task<IRepositoryStatus> _status;

        protected StatusWriter(IGitPromptSettings settings, Task<IRepositoryStatus> status)
        {
            _settings = settings;
            _status = status;
        }

        public async Task WriteStatusAsync()
        {
            if (!_settings.EnablePromptStatus)
            {
                return;
            }

            var status = await _status;

            WriteColor(_settings.BeforeText, _settings.Before);

            WriteColor(status.Branch, GetBranchColor(status));

            if (_settings.EnableFileStatus && status.Index.HasAny)
            {
                WriteColor(_settings.BeforeIndexText, _settings.BeforeIndex);

                if (_settings.ShowStatusWhenZero || status.Index.Added.Any())
                {
                    WriteColor($" +{status.Index.Added.Count}", _settings.Index);
                }

                if (_settings.ShowStatusWhenZero || status.Index.Modified.Any())
                {
                    WriteColor($" ~{status.Index.Modified.Count}", _settings.Index);
                }

                if (_settings.ShowStatusWhenZero || status.Index.Deleted.Any())
                {
                    WriteColor($" -{status.Index.Deleted.Count}", _settings.Index);
                }

                if (status.Index.Unmerged.Any())
                {
                    WriteColor($" !{status.Index.Unmerged.Count}", _settings.Index);
                }

                if (status.Working.HasAny)
                {
                    WriteColor(_settings.DelimText, _settings.Delim);
                }
            }

            if (_settings.EnableFileStatus && status.Working.HasAny)
            {
                if (_settings.ShowStatusWhenZero || status.Working.Added.Any())
                {
                    WriteColor($" +{status.Working.Added.Count}", _settings.Working);
                }

                if (_settings.ShowStatusWhenZero || status.Index.Modified.Any())
                {
                    WriteColor($" ~{status.Working.Modified.Count}", _settings.Working);
                }

                if (_settings.ShowStatusWhenZero || status.Index.Deleted.Any())
                {
                    WriteColor($" -{status.Working.Deleted.Count}", _settings.Working);
                }

                if (status.Index.Unmerged.Any())
                {
                    WriteColor($" !{status.Working.Unmerged.Count}", _settings.Working);
                }
            }

            //if (status.HasUntracked)
            //{
            //    WriteColor(_settings.UntrackedText, _settings.Untracked.Background, _settings.Untracked.Foreground);
            //}

            WriteColor(_settings.AfterText, _settings.After);

            // TODO: Update Window title
        }

        private PromptColor GetBranchColor(IRepositoryStatus status)
        {
            if (status.BehindBy > 0 && status.AheadBy > 0)
            {
                return _settings.BranchBehindAndAhead;
            }
            else if (status.BehindBy > 0)
            {
                return _settings.BranchBehind;
            }
            else if (status.AheadBy > 0)
            {
                return _settings.BranchAhead;
            }
            else
            {
                return _settings.Branch;
            }
        }

        protected abstract void WriteColor(string msg, PromptColor color);
    }
}