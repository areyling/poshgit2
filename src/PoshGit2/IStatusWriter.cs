using System.Threading.Tasks;

namespace PoshGit2
{
    public interface IStatusWriter
    {
        Task WriteStatusAsync();
    }
}