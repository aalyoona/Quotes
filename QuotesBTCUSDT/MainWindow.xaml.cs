using Quotes.Exchanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private readonly string[] _symbols;
        private CancellationTokenSource _cts;

        public MainWindow()
        {
            _symbols = Symbols.AllSymbols;
            _cts = new CancellationTokenSource();
            InitializeComponent();
            LoadingExchanges();
            FillingSymbolComboBox();
            LoadingQuotesAsync(_cts.Token);

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

        private async Task LoadingQuotesAsync(CancellationToken token)
        {
            if (FirstSymbolComboBox.SelectedIndex != -1 && SecondSymbolComboBox.SelectedIndex != -1)
            {
                string firstSymbol = FirstSymbolComboBox.SelectedValue.ToString();
                string secondSymbol = SecondSymbolComboBox.SelectedValue.ToString();

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var result = new List<QuotesModel>();
                        foreach (var exchange in _exchanges)
                        {
                            var quote = await exchange.GetQuotes(firstSymbol, secondSymbol);
                            if (quote != null)
                            {
                                result.Add(quote);
                            }
                        }

                        Dispatcher.Invoke(() =>
                        {
                            QuotesDataGrid.Items.Clear();
                            if (result.Count > 1)
                            {
                                foreach (var r in result)
                                {
                                    QuotesDataGrid.Items.Add(r);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"The current pair is currently not supported on the exchanges that are being monitored. Try swapping symbols or choosing different ones.",
                                "Pair not supported", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        });
                        await Task.Delay(5000);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"An error occurred while requesting. {e.Message} Click \"Ok\" to request a quote again.",
                            "Request error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        MainWindow mw = new MainWindow();
                        Application.Current.MainWindow = mw;
                        mw.Show();
                        Close();
                    }
                }
            }
        }

        private void FillingSymbolComboBox()
        {
            foreach (string symbol in _symbols)
            {
                FirstSymbolComboBox.Items.Add(symbol);
                SecondSymbolComboBox.Items.Add(symbol);
            }

            // Selecting a default pair BTCUSDT
            FirstSymbolComboBox.SelectedValue = _symbols[1];
            SecondSymbolComboBox.SelectedValue = _symbols[0];
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            LoadingQuotesAsync(_cts.Token);
        }
    }
}
