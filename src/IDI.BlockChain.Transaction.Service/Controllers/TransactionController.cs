using IDI.BlockChain.Common.Enums;
using IDI.BlockChain.Domain.Transaction.Services;
using IDI.BlockChain.Models.Transaction;
using IDI.Core.Common;
using Microsoft.AspNetCore.Mvc;

namespace IDI.BlockChain.Transaction.Service.Controllers
{
    [Route("api/trans")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService service;

        public TransactionController(ITransactionService service)
        {
            this.service = service;
        }

        [HttpGet("quotation/{symbol}/{range}")]
        public Result<Quotation> GetQuotation(string symbol, KLineRange range)
        {
            var quotation = new Quotation { Symbol = symbol, Success = false };
            var result = service.GetKLine(range);

            if (result.Status == ResultStatus.Success)
            {
                quotation.KLine = result.Data;
                quotation.Success = true;
            }

            return Result.Success(quotation);
        }

        [HttpPost("trade")]
        public Result Trade([FromBody]TradeInput input)
        {
            switch (input.Type)
            {
                case TranType.Bid:
                    return service.Bid(input.UID, input.Price, input.Size);
                case TranType.Ask:
                    return service.Ask(input.UID, input.Price, input.Size);
                default:
                    return Result.Fail("transaction failed.");
            }
        }

        [HttpPost("start")]
        public Result Start()
        {
            service.Start();

            return service.Running ? Result.Success("transaction service startup.") : Result.Fail("transaction service startup fail.");
        }

        [HttpPost("stop")]
        public Result Stop()
        {
            service.Stop();

            return service.Running ? Result.Success("transaction service stopped.") : Result.Fail("transaction service close failed.");
        }
    }
}
