using IDI.Core.Common;
using IDI.BlockChain.Models.Transaction;

namespace IDI.BlockChain.Domain.Transaction.Services
{
    public interface ITransactionService
    {
        bool Running { get; }

        void Start();

        void Stop();

        Result Bid(int uid, decimal price, decimal size);

        Result Ask(int uid, decimal price, decimal size);

        Result<KLine> GetKLine();
    }
}
