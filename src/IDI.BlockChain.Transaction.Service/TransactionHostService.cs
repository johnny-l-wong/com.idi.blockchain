using IDI.Core.Infrastructure;
using IDI.BlockChain.Domain.Transaction.Services;
using Microsoft.AspNetCore.Hosting;
#if NET461
using Microsoft.AspNetCore.Hosting.WindowsServices;
#endif

namespace IDI.BlockChain.Transaction.Service
{
#if NET461
    internal class TransactionHostService : WebHostService
#else
    internal class TransactionHostService : HostService
#endif
    {
        private readonly ITransactionService service;

        public TransactionHostService(IWebHost host) : base(host)
        {
            this.service = Runtime.GetService<ITransactionService>();
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }
    }
}
