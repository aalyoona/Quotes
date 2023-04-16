using Kucoin.Net.Clients;
using System.Threading.Tasks;

namespace Quotes.Exchanges
{
    internal class KucoinExchange : IExchange
    {
        private readonly KucoinClient _client;
        public KucoinExchange()
        {
            _client = new KucoinClient();
        }

        public async Task<QuotesModel> GetQuotes(string firstSymbol, string secondSymbol)
        {
            var ticker = await _client.SpotApi.ExchangeData.GetTickerAsync($"{firstSymbol}-{secondSymbol}");
            if (ticker.Data == null)
            {
                return null;
            }

            QuotesModel model = new QuotesModel()
            {
                Exchange = GetType().Name,
                Price = (decimal)ticker.Data.LastPrice,
                Symbol = $"{firstSymbol}-{secondSymbol}"
            };
            return model;
        }
    }
}
