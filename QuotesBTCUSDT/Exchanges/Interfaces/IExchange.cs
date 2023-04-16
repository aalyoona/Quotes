using System.Threading.Tasks;

namespace Quotes.Exchanges
{
    internal interface IExchange
    {
        Task<QuotesModel> GetQuotes(string firstSymbol, string secondSymbol);
    }
}
