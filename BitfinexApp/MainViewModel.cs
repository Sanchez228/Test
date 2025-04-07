using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using BitfinexConnector.Services;
using BitfinexConnector;


public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<BalanceEntry> Portfolio { get; private set; } = new ObservableCollection<BalanceEntry>();


    public MainViewModel()
    {
        Portfolio = new ObservableCollection<BalanceEntry>();
        _ = LoadPortfolioAsync(); // можно просто вызвать без "_ ="
    }

    private async Task LoadPortfolioAsync()
    {
        var balances = new Dictionary<string, decimal>
        {
            { "BTC", 1 },
            { "XRP", 15000 },
            { "XMR", 50 },
            { "DASH", 30 }
        };

        var client = new BitfinexRestClient();
        var calculator = new PortfolioCalculator(client);
        var results = await calculator.CalculateAsync(balances);

        Portfolio = new ObservableCollection<BalanceEntry>(results);
        OnPropertyChanged(nameof(Portfolio));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
