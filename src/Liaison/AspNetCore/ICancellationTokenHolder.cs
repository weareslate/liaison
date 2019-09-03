using System.Threading;

namespace Liaison.AspNetCore
{
    public interface ICancellationTokenHolder
    {
        CancellationToken GetCancellationToken();
    }
}