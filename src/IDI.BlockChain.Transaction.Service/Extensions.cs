﻿#if NET461
using System.ServiceProcess;
using Microsoft.AspNetCore.Hosting;
#endif

namespace IDI.BlockChain.Transaction.Service
{
    public static class Extensions
    {
#if NET461
        public static void RunAsWindowsService(this IWebHost host)
        {
            ServiceBase.Run(new TransactionHostService(host));
        }
#endif
    }
}
