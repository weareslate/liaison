using System.Threading;

namespace Liaison.AspNetCore
{
    public class CancellationTokenHolder : ICancellationTokenHolder
    {
        private readonly CancellationToken token;

        public CancellationTokenHolder(
            CancellationToken token)
        {
            this.token = token;
        }
        
        public CancellationToken GetCancellationToken()
        {
            return this.token;
        }
    }
}