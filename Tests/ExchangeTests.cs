using Shouldly;
using Xunit;

namespace Tests;

public class ExchangeTests
    {
    public static TheoryData<string, decimal> ParseCurrencyTestData => new () 
        {
        {"EUR", Rates.Eur},
        {"USD", Rates.Usd},
        {"GBP", Rates.Gbp},
        {"SEK", Rates.Sek},
        {"NOK", Rates.Nok},
        {"CHF", Rates.Chf},
        {"JPY", Rates.Jpy},
        {"DKK", Rates.Dkk},
        };

    public static TheoryData<decimal, decimal, decimal> ClaculateRateTestData => new () 
        {
        {Rates.Eur, Rates.Dkk, Rates.Eur / Rates.Dkk},
        {Rates.Usd, Rates.Eur, Rates.Usd / Rates.Eur},
        {Rates.Gbp, Rates.Sek, Rates.Gbp / Rates.Sek},
        {1m, 2m, 0.5m},
        {1m, 1m, 1m},
        {10m, 5m, 2m}
        };

    public static TheoryData<decimal, decimal, decimal> ExchangeWithRateTestData => new () 
        {
        {1m, 2m, 2m},
        {1m, 1m, 1m},
        {10m, 5m, 50m}
        };

    [Theory]
    [MemberData (nameof (ParseCurrencyTestData))]
    public void ParseCurrency_WhenPassingCurrency_ShouldReturnCorrectRate (string currencyString, decimal rate)
        {
        // Assume
        // Act
        var result = Exchange.ParseCurrency (currencyString);

        // Assert
        result.ShouldBe (rate);
        }

    [Fact]
    public void ParseCurrency_WhenPassingNonCurrency_ShouldThrow ()
        {
        // Assume
        var junk = "asddas";

        // Act
        // Assert
        var e = Should.Throw<Exception> (() => Exchange.ParseCurrency (junk));
        e.Message.ShouldBe ("Currency asddas not supported");
        }

    [Fact]
    public void ParseInput_WhenPassingInput_ShouldCorrectlyPase ()
        {
        // Assume
        string[] args = {"EUR/USD", "123,19"};

        // Act
        Exchange.ParseInput (args, out var from, out var to, out var amount);

        // Assert
        from.ShouldBe ("EUR");
        to.ShouldBe ("USD");
        amount.ShouldBe (123.19m);
        }

    [Theory]
    [InlineData ("EUR/USD", "asd")]
    [InlineData ("EUR", "123,19")]
    [InlineData ("12", "123,19")]
    [InlineData ("EUR/USD/GBP", "123,19")]
    public void ParseInput_WhenPassingIncorrectInput_ShouldThrow (params string[] args)
        {
        // Assume
        // Act
        // Assert
        var e = Should.Throw<Exception> (() => Exchange.ParseInput (args, out var from, out var to, out var amount));
        e.Message.ShouldBe ("Usage: Exchange <currency pair> <amount to exchange>");
        }

    [Theory]
    [MemberData (nameof (ClaculateRateTestData))]
    public void ClaculateRate_WhenCalled_ShouldCorrectlyCalculateRate (decimal from, decimal to, decimal rate)
        {
        // Assume
        // Act
        var result = Exchange.ClaculateRate (from, to);

        // Assert
        result.ShouldBe (rate);
        }

    [Theory]
    [MemberData (nameof (ExchangeWithRateTestData))]
    public void ExchangeWithRate_WhenCalled_ShouldCorrectlyExchange (decimal rate, decimal amount, decimal exchanged)
        {
        // Assume
        // Act
        var result = Exchange.ExchangeWithRate (rate, amount);

        // Assert
        result.ShouldBe (exchanged);
        }


    [Theory]
    [InlineData ("7,4394", "EUR/DKK", "1")]
    [InlineData ("743,9400", "EUR/DKK", "100")]
    [InlineData ("0,8722987629712141642727326025", "EUR/GBP", "1")]
    [InlineData ("0,1344194424281528080221523241", "DKK/EUR", "1")]
    public void RunExchange_WhenCalledCorrectly_ShouldPrintCorrectResult (string result, params string[] args)
        {
        // Assume
        result += Environment.NewLine;

        var stringWriter = new StringWriter ();
        Console.SetOut (stringWriter);

        // Act
        Exchange.RunExchange (args);

        // Assert
        stringWriter.ToString ().ShouldBe (result);
        }

    [Theory]
    [InlineData ("Usage: Exchange <currency pair> <amount to exchange>", "EUR/DKK/GBP", "1")]
    [InlineData ("Usage: Exchange <currency pair> <amount to exchange>", "EUR/DKK", "asd")]
    [InlineData ("Usage: Exchange <currency pair> <amount to exchange>", "EUR", "1")]
    [InlineData ("Currency ASD not supported", "EUR/ASD", "1")]
    public void RunExchange_WhenCalledIncorrectly_ShouldThrowCorrectException (string message, params string[] args)
        {
        // Assume
        // Act
        // Assert
        var e = Should.Throw<Exception> (() => Exchange.RunExchange (args));

        e.Message.ShouldBe (message);
        }
    }