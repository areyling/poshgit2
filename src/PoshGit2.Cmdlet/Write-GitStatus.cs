using System.Management.Automation;

namespace PoshGit2
{
    [Cmdlet(VerbsCommunications.Write, "GitStatus")]
    public class Write_GitStatus : AutofacCmdlet
    {
        public IStatusWriter Writer { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            Writer.WriteStatusAsync().Wait();
        }
    }
}
