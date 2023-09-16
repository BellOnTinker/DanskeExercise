internal class Exchange
    {
    internal static void Main (string[] args)
        {
        try { RunExchange (args); }
        catch (Exception e) { Console.WriteLine (e.Message); }
        }

    public static void RunExchange (string[] args)
        {
        if (args.Length != 2) { throw new Exception ("Usage: Exchange <currency pair> <amount to exchange>"); }

        ParseInput (args, out var from, out var to, out var amount);
        var rateFrom = ParseCurrency (from);
        var rateTo = ParseCurrency (to);
        var rate = ClaculateRate (rateFrom, rateTo);
        var exchanged = ExchangeWithRate (rate, amount);
        
        Console.WriteLine (exchanged);
        }

    public static decimal ExchangeWithRate (decimal rate, decimal amount)
        {
        return rate * amount;
        }

    public static decimal ClaculateRate (decimal from, decimal to)
        {
        return from / to;
        }

    public static void ParseInput (string[] args, out string from, out string to, out decimal amount)
        {
        var currecncies = args[0].Split ('/');

        if (currecncies.Length != 2) { throw new Exception ("Usage: Exchange <currency pair> <amount to exchange>"); }

        from = currecncies[0];
        to = currecncies[1];

        try { amount = decimal.Parse (args[1]); }
        catch (Exception) { throw new Exception ("Usage: Exchange <currency pair> <amount to exchange>"); }
        }

    public static decimal ParseCurrency (string currency)
        {
        return currency switch
            {
            "EUR" => Rates.Eur,
            "USD" => Rates.Usd,
            "GBP" => Rates.Gbp,
            "SEK" => Rates.Sek,
            "NOK" => Rates.Nok,
            "CHF" => Rates.Chf,
            "JPY" => Rates.Jpy,
            "DKK" => Rates.Dkk,
            _ => throw new Exception($"Currency {currency} not supported"),
            };
    }

}
