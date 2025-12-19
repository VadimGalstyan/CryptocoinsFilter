using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;


namespace CryptocoinsFilter.Background_Tasks
{


    class ProgramLogic
    {
        public async Task runTask()
        {
            string connectionString = "Server=LAPTOP-EK1FURHH\\SQLEXPRESS;Database=CryptoExchanges;Trusted_Connection=True;Encrypt=False;";

            var exchanges = new[]
            {
                new { Name="Binance", SpotApi="https://api.binance.com/api/v3/exchangeInfo", FuturesApi="https://fapi.binance.com/fapi/v1/exchangeInfo" },
                new { Name="Bybit", SpotApi="https://api.bybit.com/v5/market/instruments-info?category=spot", FuturesApi="https://api.bybit.com/v5/market/instruments-info?category=linear" },
                new { Name="Bitget", SpotApi="https://api.bitget.com/api/v2/spot/public/symbols", FuturesApi="https://api.bitget.com/api/v2/mix/market/contracts?productType=USDT-FUTURES" },                                   new { Name="Gate", SpotApi="https://api.gateio.ws/api/v4/spot/currency_pairs", FuturesApi="https://api.gateio.ws/api/v4/futures/usdt/contracts" },
                new { Name="MEXC", SpotApi="https://api.mexc.com/api/v3/exchangeInfo", FuturesApi="https://contract.mexc.com/api/v1/contract/detail" }
            };

            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            foreach (var ex in exchanges)
            {
                string spotTable = $"{ex.Name}_Spot";
                string futTable = $"{ex.Name}_Futures";

                await CreateTableIfNotExists(conn, spotTable);
                await CreateTableIfNotExists(conn, futTable);

                await ClearTable(conn, spotTable);
                await ClearTable(conn, futTable);

                // Spot
                try
                {
                    await ImportSymbols(ex.SpotApi, spotTable, conn, ex.Name);
                    Console.WriteLine($"{ex.Name} Spot Done");
                }
                catch (Exception exSpot)
                {
                    Console.WriteLine($"Error Spot {ex.Name}: {exSpot.Message}");
                }

                // Futures
                try
                {
                    await ImportFutures(ex.FuturesApi, futTable, conn, ex.Name);
                    Console.WriteLine($"{ex.Name} Futures Done");
                }
                catch (Exception exFut)
                {
                    Console.WriteLine($"Error Futures {ex.Name}: {exFut.Message}");
                }
            }

            Console.WriteLine("All imports are done!");
        }

        static async Task CreateTableIfNotExists(SqlConnection conn, string tableName)
        {
            var cmd = new SqlCommand($@"
    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tableName}' AND xtype='U')
    CREATE TABLE {tableName} (
        Id INT IDENTITY PRIMARY KEY,
        Symbol NVARCHAR(50),
        BaseAsset NVARCHAR(50),
        QuoteAsset NVARCHAR(50),
        Status NVARCHAR(50)
    );", conn);

            await cmd.ExecuteNonQueryAsync();
        }

        static async Task ClearTable(SqlConnection conn, string tableName)
        {
            var cmd = new SqlCommand($"DELETE FROM {tableName};", conn);
            await cmd.ExecuteNonQueryAsync();
        }

        static async Task ImportSymbols(string apiUrl, string tableName, SqlConnection conn, string exchangeName)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.UserAgent.ParseAdd("csharp-exchange-client/1.0");
            http.Timeout = TimeSpan.FromSeconds(30);

            var resp = await http.GetStringAsync(apiUrl);
            var symbols = new List<(string Symbol, string BaseAsset, string QuoteAsset, string Status)>();

            try
            {
                switch (exchangeName)
                {
                    case "Binance":
                        var binanceJson = JObject.Parse(resp);
                        var binanceSymbols = binanceJson["symbols"] as JArray;
                        if (binanceSymbols != null)
                        {
                            foreach (var s in binanceSymbols)
                            {
                                symbols.Add((
                                    s["symbol"]?.ToString() ?? "",
                                    s["baseAsset"]?.ToString() ?? "",
                                    s["quoteAsset"]?.ToString() ?? "",
                                    s["status"]?.ToString() ?? "TRADING"
                                ));
                            }
                        }
                        break;

                    case "Bybit":
                        var bybitJson = JObject.Parse(resp);
                        var bybitResult = bybitJson["result"];
                        if (bybitResult != null)
                        {
                            var bybitList = bybitResult["list"] as JArray;
                            if (bybitList != null)
                            {
                                foreach (var s in bybitList)
                                {
                                    symbols.Add((
                                        s["symbol"]?.ToString() ?? "",
                                        s["baseCoin"]?.ToString() ?? "",
                                        s["quoteCoin"]?.ToString() ?? "",
                                        s["status"]?.ToString() ?? "Trading"
                                    ));
                                }
                            }
                        }
                        break;

                    case "Bitget":
                        var bitgetJson = JObject.Parse(resp);
                        var bitgetData = bitgetJson["data"] as JArray;
                        if (bitgetData != null)
                        {
                            foreach (var s in bitgetData)
                            {
                                symbols.Add((
                                    s["symbolName"]?.ToString() ?? s["symbol"]?.ToString() ?? "",
                                    s["baseCoin"]?.ToString() ?? "",
                                    s["quoteCoin"]?.ToString() ?? "",
                                    s["status"]?.ToString() ?? "online"
                                ));
                            }
                        }
                        break;

                    case "Gate":
                        var gateArray = JArray.Parse(resp);
                        foreach (var s in gateArray)
                        {
                            symbols.Add((
                                s["id"]?.ToString() ?? "",
                                s["base"]?.ToString() ?? "",
                                s["quote"]?.ToString() ?? "",
                                s["trade_status"]?.ToString() == "tradable" ? "TRADING" : "DELISTED"
                            ));
                        }
                        break;

                    case "MEXC":
                        var mexcJson = JObject.Parse(resp);
                        var mexcSymbols = mexcJson["symbols"] as JArray;
                        if (mexcSymbols != null)
                        {
                            foreach (var s in mexcSymbols)
                            {
                                symbols.Add((
                                    s["symbol"]?.ToString() ?? "",
                                    s["baseAsset"]?.ToString() ?? "",
                                    s["quoteAsset"]?.ToString() ?? "",
                                    s["status"]?.ToString() ?? "TRADING"
                                ));
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Spot parsing error for {exchangeName}: {ex.Message}");
                return;
            }

            foreach (var (symbol, baseAsset, quoteAsset, status) in symbols)
            {
                try
                {
                    var cmd = new SqlCommand(
                        $"INSERT INTO {tableName} (Symbol, BaseAsset, QuoteAsset, Status) VALUES (@Symbol,@BaseAsset,@QuoteAsset,@Status)",
                        conn
                    );

                    cmd.Parameters.AddWithValue("@Symbol", symbol);
                    cmd.Parameters.AddWithValue("@BaseAsset", baseAsset);
                    cmd.Parameters.AddWithValue("@QuoteAsset", quoteAsset);
                    cmd.Parameters.AddWithValue("@Status", status);

                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Spot symbol adding error {symbol}: {ex.Message}");
                }
            }
        }

        static async Task ImportFutures(string apiUrl, string tableName, SqlConnection conn, string exchangeName)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.UserAgent.ParseAdd("csharp-exchange-client/1.0");
            http.Timeout = TimeSpan.FromSeconds(30);

            var resp = await http.GetStringAsync(apiUrl);
            var symbols = new List<(string Symbol, string BaseAsset, string QuoteAsset, string Status)>();

            try
            {
                switch (exchangeName)
                {
                    case "Binance":
                        var binanceJson = JObject.Parse(resp);
                        var binanceSymbols = binanceJson["symbols"] as JArray;
                        if (binanceSymbols != null)
                        {
                            foreach (var s in binanceSymbols)
                            {
                                symbols.Add((
                                    s["symbol"]?.ToString() ?? "",
                                    s["baseAsset"]?.ToString() ?? "",
                                    s["quoteAsset"]?.ToString() ?? "",
                                    s["status"]?.ToString() ?? "TRADING"
                                ));
                            }
                        }
                        break;

                    case "Bybit":
                        var bybitJson = JObject.Parse(resp);
                        var bybitResult = bybitJson["result"];
                        if (bybitResult != null)
                        {
                            var bybitList = bybitResult["list"] as JArray;
                            if (bybitList != null)
                            {
                                foreach (var s in bybitList)
                                {
                                    symbols.Add((
                                        s["symbol"]?.ToString() ?? "",
                                        s["baseCoin"]?.ToString() ?? "",
                                        s["quoteCoin"]?.ToString() ?? "",
                                        s["status"]?.ToString() ?? "Trading"
                                    ));
                                }
                            }
                        }
                        break;

                    case "Bitget":
                        var bitgetJson = JObject.Parse(resp);
                        var bitgetData = bitgetJson["data"] as JArray;
                        if (bitgetData != null)
                        {
                            foreach (var s in bitgetData)
                            {
                                symbols.Add((
                                    s["symbol"]?.ToString() ?? "",
                                    s["baseCoin"]?.ToString() ?? "",
                                    s["quoteCoin"]?.ToString() ?? "",
                                    s["status"]?.ToString() ?? "normal"
                                ));
                            }
                        }
                        break;


                    case "Gate":
                        JArray gateArray;

                        if (resp.TrimStart().StartsWith("["))
                            gateArray = JArray.Parse(resp);
                        else
                            gateArray = JArray.Parse(JObject.Parse(resp)["data"]!.ToString());

                        foreach (var s in gateArray)
                        {
                            string name = s["name"]?.ToString() ?? "";

                            string baseAsset = "";
                            string quoteAsset = "";

                            if (name.Contains("_"))
                            {
                                var parts = name.Split("_");
                                baseAsset = parts[0];
                                quoteAsset = parts[1];
                            }

                            string statusRaw = s["status"]?.ToString()?.ToLower() ?? "trading";
                            string status = statusRaw == "trading" ? "TRADING" : "DELISTED";

                            symbols.Add((name, baseAsset, quoteAsset, status));
                        }
                        break;

                    case "MEXC":
                        var mexcJson = JObject.Parse(resp);
                        var mexcDataArray = mexcJson["data"] as JArray;
                        if (mexcDataArray != null)
                        {
                            foreach (var s in mexcDataArray)
                            {
                                symbols.Add((
                                    s["symbol"]?.ToString() ?? "",
                                    s["baseCoin"]?.ToString() ?? "",
                                    s["quoteCoin"]?.ToString() ?? "USDT",
                                    s["state"]?.ToString() == "1" ? "TRADING" : "DELISTED"
                                ));
                            }
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Futures parsing error for {exchangeName}: {ex.Message}");
                return;
            }

            foreach (var (symbol, baseAsset, quoteAsset, status) in symbols)
            {
                try
                {
                    var cmd = new SqlCommand(
                        $"INSERT INTO {tableName} (Symbol, BaseAsset, QuoteAsset, Status) VALUES (@Symbol,@BaseAsset,@QuoteAsset,@Status)",
                        conn
                    );

                    cmd.Parameters.AddWithValue("@Symbol", symbol);
                    cmd.Parameters.AddWithValue("@BaseAsset", baseAsset);
                    cmd.Parameters.AddWithValue("@QuoteAsset", quoteAsset);
                    cmd.Parameters.AddWithValue("@Status", status);

                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Futures symbol adding error {symbol}: {ex.Message}");
                }
            }
        }
    }

}