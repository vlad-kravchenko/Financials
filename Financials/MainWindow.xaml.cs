using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Financials
{
    public partial class MainWindow : Window
    {
        List<FinSymbol> symbols = new List<FinSymbol>();

        public MainWindow()
        {
            InitializeComponent();

            symbols.Add(new FinSymbol { Control = usd, Id = true, Address = "https://ru.investing.com/currencies/usd-uah", OnPage = "last_last" });
            symbols.Add(new FinSymbol { Control = eur, Id = true, Address = "https://ru.investing.com/currencies/eur-uah", OnPage = "last_last" });
            symbols.Add(new FinSymbol { Control = eth, Id = true, Address = "https://ru.investing.com/crypto/ethereum", OnPage = "last_last" });
            symbols.Add(new FinSymbol { Control = btc, Id = true, Address = "https://ru.investing.com/crypto/bitcoin/btc-usd", OnPage = "last_last" });
            symbols.Add(new FinSymbol { Control = brent, Id = true, Address = "https://ru.investing.com/commodities/brent-oil", OnPage = "last_last" });
            symbols.Add(new FinSymbol { Control = gold, Id = true, Address = "https://ru.investing.com/commodities/gold", OnPage = "last_last" });
            symbols.Add(new FinSymbol { Control = silver, Id = true, Address = "https://ru.investing.com/commodities/silver", OnPage = "last_last" });
            symbols.Add(new FinSymbol { Control = sp500, Id = true, Address = "https://ru.investing.com/indices/us-spx-500-futures", OnPage = "last_last" });
            symbols.Add(new FinSymbol { Control = amzn, Id = false, Address = "https://ru.investing.com/equities/amazon-com-inc", OnPage = "instrument-price_last__KQzyA" });
            symbols.Add(new FinSymbol { Control = aapl, Id = false, Address = "https://ru.investing.com/equities/apple-computer-inc", OnPage = "instrument-price_last__KQzyA" });
            symbols.Add(new FinSymbol { Control = googl, Id = false, Address = "https://ru.investing.com/equities/google-inc", OnPage = "instrument-price_last__KQzyA" });
            symbols.Add(new FinSymbol { Control = msft, Id = false, Address = "https://ru.investing.com/equities/microsoft-corp", OnPage = "instrument-price_last__KQzyA" });
            symbols.Add(new FinSymbol { Control = jnj, Id = false, Address = "https://ru.investing.com/equities/johnson-johnson", OnPage = "instrument-price_last__KQzyA" });

            Parallel.ForEach(symbols, async item =>
            {
                using (WebClient client = new WebClient())
                {
                    var config = Configuration.Default.WithDefaultLoader();
                    var context = BrowsingContext.New(config);

                    var document = await context.OpenAsync(item.Address);
                    IElement element;
                    if (item.Id)
                        element = document.GetElementById(item.OnPage);
                    else
                        element = document.GetElementsByClassName(item.OnPage)[0];
                    string text = element.TextContent.Replace(".", "");
                    double value;
                    if (double.TryParse(text, out value))
                    {
                        value = Math.Round(value, 2);
                        text = value.ToString("F2");
                    }
                    else text = "Error";
                    Dispatcher.Invoke((Action)(() => item.Control.Text = text ));
                }
            });
        }
    }
}