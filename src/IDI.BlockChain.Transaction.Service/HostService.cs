using Microsoft.AspNetCore.Hosting;

namespace IDI.BlockChain.Transaction.Service
{
    internal abstract class HostService
    {
        public HostService(IWebHost host) { }

        protected virtual void OnStarting(string[] args) { }

        protected virtual void OnStarted() { }

        protected virtual void OnStopped() { }
    }
}