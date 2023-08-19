using System.Threading.Tasks;

namespace CitadelShowdown.Common.Abstract
{
    public interface ICompletable
    {
        Task Complete(bool isSuccessful = true);
    }
}
