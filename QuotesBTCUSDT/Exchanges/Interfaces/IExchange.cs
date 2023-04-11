using System.Threading.Tasks;

namespace Quotes.Exchanges
{
    internal interface IExchange
    {
        Task<QuotesModel> GetQuotesBTCUSDT();
    }
}
