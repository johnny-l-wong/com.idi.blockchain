using System;
using IDI.Core.Common;
using IDI.Core.Extensions;
using IDI.Core.Logging;
using IDI.BlockChain.Common.Enums;
using IDI.BlockChain.Models.Base;
using IDI.BlockChain.Models.Transaction;

namespace IDI.BlockChain.Domain.Transaction.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger logger;
        private readonly TradeDrive drive;
        private readonly Guid id;

        public bool Running => drive.Running;

        public TransactionService(ILogger logger)
        {
            id = Guid.NewGuid();
            this.logger = logger;
            drive = new TradeDrive();
            drive.DriveStarted += OnStarted;
            drive.DriveStopped += OnStopped;
            drive.BidEnqueue += OnBidEnqueue;
            drive.AskEnqueue += OnAskEnqueue;
            drive.TradeCompleted += OnTradeCompleted;
            drive.Start();
        }

        #region Events
        private void OnTradeCompleted(TradeResult result)
        {
            if (result.Logs.Count > 0)
                logger.Info($"trade:{result.ToJson()}");
        }

        private void OnAskEnqueue(TradeOrder order)
        {
            logger.Info($"ask:{order.ToJson()}");
        }

        private void OnBidEnqueue(TradeOrder order)
        {
            logger.Info($"bid:{order.ToJson()}");
        }

        private void OnStopped()
        {
            logger.Info($"transaction service {id} stopped");
        }

        private void OnStarted()
        {
            logger.Info($"transaction service {id} started");
        }
        #endregion

        public Result Ask(int uid, decimal price, decimal size)
        {
            drive.Ask(uid, price, size);

            return Result.Success("ask success.");
        }

        public Result Bid(int uid, decimal price, decimal size)
        {
            drive.Bid(uid, price, size);

            return Result.Success("bid success.");
        }

        public Result<KLine> GetKLine(KLineRange range)
        {
            return Result.Success(drive.GetKLine(range));
        }

        public void Start()
        {
            drive.Start();
        }

        public void Stop()
        {
            drive.Stop();
        }
    }
}
