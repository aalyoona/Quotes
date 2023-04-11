using Quotes.Exchanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Quotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<IExchange> _exchanges = new List<IExchange>();
        public MainWindow()
        {
            InitializeComponent();
            LoadingExchanges();
            LoadingQuotes();
        }
        private void LoadingExchanges()
        {
            var type = typeof(IExchange);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface).ToList();

            foreach (var t in types)
            {
                _exchanges.Add((IExchange)Activator.CreateInstance(t));
            }
        }
        private async void LoadingQuotes()
        {
            while (true)
            {
                try
                {
                    var result = new List<QuotesModel>();
                    foreach (var exchange in _exchanges)
                    {
                        result.Add(await exchange.GetQuotesBTCUSDT());
                    }

                    Dispatcher.Invoke(() =>
                    {
                        QuotesDataGrid.Items.Clear();
                        foreach (var r in result)
                        {
                            QuotesDataGrid.Items.Add(r);
                        }
                    });
                    await Task.Delay(5000);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"An error occurred while requesting. {e.Message} Click \"Ok\" to request a quote again.", "Request error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MainWindow mw = new MainWindow();
                    Application.Current.MainWindow = mw;
                    mw.Show();
                    Close();
                }
            }
        }
    }
}
