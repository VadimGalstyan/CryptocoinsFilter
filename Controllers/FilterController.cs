using CryptocoinsFilter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CryptocoinsFilter.Background_Tasks;

namespace LearnCryptocoinsFilteringProject.Controllers
{
    public class FilterController : Controller
    {
        private readonly string connectionString = "Server=LAPTOP-EK1FURHH\\SQLEXPRESS;Database=CryptoExchanges;Trusted_Connection=True;Encrypt=False;";

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Filter());
        }

        string chosen(bool include)
        {
            return include ? "" : "NOT ";
        }

        string mainExchange(Filter model)//returns tha exchange which is choosed by user
        {
            if (model.BinanceFutures == true)
            {
                return "Binance_Futures";
            }
            if (model.BinanceSpot == true)
            {
                return "Binance_Spot";
            }
            if (model.BybitFutures == true)
            {
                return "Bybit_Futures";
            }
            if (model.BybitSpot == true)
            {
                return "Bybit_Spot";
            }
            if (model.BitgetFutures == true)
            {
                return "Bitget_Futures";
            }
            if (model.BitgetSpot == true)
            {
                return "Bitget_Spot";
            }
            if (model.GateFutures == true)
            {
                return "Gate_Futures";
            }
            if (model.GateSpot == true)
            {
                return "Gate_Spot";
            }
            if (model.MexcFutures == true)
            {
                return "Mexc_Futures";
            }
            if (model.MexcSpot == true)
            {
                return "Mexc_Spot";
            }

            return "NONE";
        }

        string MainDisabledExchange(Filter model) //returns the exchange that user disabled.Calling when no  exchange are enabled
        {
            if (model.BinanceFutures == false)
            {
                return "Binance_Futures";
            }
            if (model.BinanceSpot == false)
            {
                return "Binance_Spot";
            }
            if (model.BybitFutures == false)
            {
                return "Bybit_Futures";
            }
            if (model.BybitSpot == false)
            {
                return "Bybit_Spot";
            }
            if (model.BitgetFutures == false)
            {
                return "Bitget_Futures";
            }
            if (model.BitgetSpot == false)
            {
                return "Bitget_Spot";
            }
            if (model.GateFutures == false)
            {
                return "Gate_Futures";
            }
            if (model.GateSpot == false)
            {
                return "Gate_Spot";
            }
            if (model.MexcFutures == false)
            {
                return "Mexc_Futures";
            }
            if (model.MexcSpot == false)
            {
                return "Mexc_Spot";
            }

            return "NONE";
        }

        bool noOneChoosed(Filter model)
        {
            if (model.BinanceFutures == false && model.BinanceSpot == false && model.BybitFutures == false && model.BybitSpot == false
                && model.BitgetFutures == false && model.BitgetSpot == false && model.GateFutures == false && model.GateSpot == false
               && model.MexcFutures == false && model.MexcSpot == false)
            {
                return true;
            }
            return false;
        }
        string AddAnd(string query)
        {
            if (query != "")
            {
                return " AND ";
            }
            return "";
        }

        string AddUnion(string query)
        {
            if (query != "SELECT s.Symbol, s.BaseAsset, s.QuoteAsset FROM(")
            {
                return @" 
                        Union 
                        ";
            }
            return "";
        }

        string AddAndForDisabled(string query)
        {
            if (query != " WHERE ")
            {
                return " AND ";
            }
            return "";
        }

        string ExchangesOneEnabled(Filter model) //This function constructing the query when at least one exchange is enabled
        {
            string query = "";
            if (model.BinanceFutures == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Binance_Futures binanceF
                    WHERE binanceF.BaseAsset = mf.BaseAsset
                      AND binanceF.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.BinanceFutures == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Binance_Futures binanceF
                    WHERE binanceF.BaseAsset = mf.BaseAsset
                      AND binanceF.QuoteAsset = mf.QuoteAsset
                )";
                }
            }

            if (model.BinanceSpot == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Binance_Spot binanceS
                    WHERE binanceS.BaseAsset = mf.BaseAsset
                      AND binanceS.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.BinanceSpot == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Binance_Spot binanceS
                    WHERE binanceS.BaseAsset = mf.BaseAsset
                      AND binanceS.QuoteAsset = mf.QuoteAsset
                )";
                }
            }


            if (model.BybitFutures == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Bybit_Futures bybitf
                    WHERE bybitf.BaseAsset = mf.BaseAsset
                      AND bybitf.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.BybitFutures == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Bybit_Futures bybitf
                    WHERE bybitf.BaseAsset = mf.BaseAsset
                      AND bybitf.QuoteAsset = mf.QuoteAsset
                    )";
                }
            }

            if (model.BybitSpot == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Bybit_Spot bybitS
                    WHERE bybitS.BaseAsset = mf.BaseAsset
                      AND bybitS.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.BybitSpot == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Bybit_Spot bybitS
                    WHERE bybitS.BaseAsset = mf.BaseAsset
                      AND bybitS.QuoteAsset = mf.QuoteAsset
                    )";
                }
            }

            if (model.BitgetFutures == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Bitget_Futures bitgetF
                    WHERE bitgetF.BaseAsset = mf.BaseAsset
                      AND bitgetF.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.BitgetFutures == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Bitget_Futures bitgetF
                    WHERE bitgetF.BaseAsset = mf.BaseAsset
                      AND bitgetF.QuoteAsset = mf.QuoteAsset
                    )";
                }
            }

            if (model.BitgetSpot == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Bitget_Spot bitgetS
                    WHERE bitgetS.BaseAsset = mf.BaseAsset
                      AND bitgetS.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.BitgetSpot == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Bitget_Spot bitgetS
                    WHERE bitgetS.BaseAsset = mf.BaseAsset
                      AND bitgetS.QuoteAsset = mf.QuoteAsset
                    )";
                }
            }

            if (model.GateFutures == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Gate_Futures GateF
                    WHERE GateF.BaseAsset = mf.BaseAsset
                      AND GateF.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.GateFutures == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Gate_Futures GateF
                    WHERE GateF.BaseAsset = mf.BaseAsset
                      AND GateF.QuoteAsset = mf.QuoteAsset
                    )";
                }
            }

            if (model.GateSpot == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Gate_Spot GateS
                    WHERE GateS.BaseAsset = mf.BaseAsset
                      AND GateS.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.GateSpot == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Gate_Spot GateS
                    WHERE GateS.BaseAsset = mf.BaseAsset
                      AND GateS.QuoteAsset = mf.QuoteAsset
                    )";
                }
            }

            if (model.MexcFutures == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Mexc_Futures MexcF
                    WHERE MexcF.BaseAsset = mf.BaseAsset
                      AND MexcF.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.MexcFutures == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Mexc_Futures MexcF
                    WHERE MexcF.BaseAsset = mf.BaseAsset
                      AND MexcF.QuoteAsset = mf.QuoteAsset
                    )";
                }
            }

            if (model.MexcSpot == true)
            {
                query += AddAnd(query);
                query += @"EXISTS (
                    SELECT 1 FROM Mexc_Spot MexcS
                    WHERE MexcS.BaseAsset = mf.BaseAsset
                      AND MexcS.QuoteAsset = mf.QuoteAsset
                )";
            }
            else
            {
                if (model.MexcSpot == false)
                {
                    query += AddAnd(query);
                    query += @"NOT EXISTS (
                    SELECT 1 FROM Mexc_Spot MexcS
                    WHERE MexcS.BaseAsset = mf.BaseAsset
                      AND MexcS.QuoteAsset = mf.QuoteAsset
                    )";
                }
            }

            query += ';';

            return query;
        }

        string DisabledExchanges(Filter model)// This function construct the query when no one exchange is enabled and there are only disabled exchanges
        {
            string query = "SELECT s.Symbol, s.BaseAsset, s.QuoteAsset FROM(";//main query
            string queryTemp = " WHERE ";//the second part of query,in the past we gonna split 

            if (model.BinanceFutures != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Binance_Futures";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                                SELECT 1
                                FROM Binance_Futures binF
                                WHERE binF.BaseAsset = s.BaseAsset
                                  AND binF.QuoteAsset = s.QuoteAsset
                                )";
            }

            if (model.BinanceSpot != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Binance_Spot";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Binance_Spot binS
                            WHERE binS.BaseAsset = s.BaseAsset
                                AND binS.QuoteAsset = s.QuoteAsset
                            )";

            }


            if (model.BybitFutures != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Bybit_Futures";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Bybit_Futures bybF
                            WHERE bybF.BaseAsset = s.BaseAsset
                                AND bybF.QuoteAsset = s.QuoteAsset
                            )";

            }


            if (model.BybitSpot != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Bybit_Spot";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Bybit_Spot bybS
                            WHERE bybS.BaseAsset = s.BaseAsset
                                AND bybS.QuoteAsset = s.QuoteAsset
                            )";

            }


            if (model.BitgetFutures != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Bitget_Futures";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Bitget_Futures bitgF
                            WHERE bitgF.BaseAsset = s.BaseAsset
                                AND bitgF.QuoteAsset = s.QuoteAsset
                            )";

            }


            if (model.BitgetSpot != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Bitget_Spot";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Bitget_Spot bitgS
                            WHERE bitgS.BaseAsset = s.BaseAsset
                                AND bitgS.QuoteAsset = s.QuoteAsset
                            )";

            }


            if (model.GateFutures != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Gate_Futures";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Gate_Futures gateF
                            WHERE gateF.BaseAsset = s.BaseAsset
                                AND gateF.QuoteAsset = s.QuoteAsset
                            )";

            }


            if (model.GateSpot != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Gate_Spot";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Gate_Spot gateS
                            WHERE gateS.BaseAsset = s.BaseAsset
                                AND gateS.QuoteAsset = s.QuoteAsset
                            )";

            }


            if (model.MexcFutures != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Mexc_Futures";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Mexc_Futures mexcF
                            WHERE mexcF.BaseAsset = s.BaseAsset
                                AND mexcF.QuoteAsset = s.QuoteAsset
                            )";

            }

            if (model.MexcSpot != false)
            {
                query += AddUnion(query);
                query += "SELECT Symbol, BaseAsset, QuoteAsset FROM Mexc_Spot";
            }
            else
            {
                queryTemp += AddAndForDisabled(queryTemp);
                queryTemp += @" NOT EXISTS(
                            SELECT 1
                            FROM Mexc_Spot mexcS
                            WHERE mexcS.BaseAsset = s.BaseAsset
                                AND mexcS.QuoteAsset = s.QuoteAsset
                            )";

            }

            queryTemp += ";";
            query += ") AS s ";
            query += queryTemp;

            return query;

        }

        [HttpPost]
        public IActionResult Apply(Filter model)
        {
            if (UpdateStatus.IsUpdating)
            {

                ViewBag.Message = "Coin update in progress, please try again after 2 minutes.It is doing for adding new listings and remove delisted coins.";
                return View("Index", model);
            }

            if(noOneChoosed(model))
            {
                return View("Index", model);
            }

            string query = "";


            if (mainExchange(model) != "NONE")
            {
                //Console.WriteLine("hello");
                query =
                   @"SELECT mf.Symbol, mf.BaseAsset, mf.QuoteAsset
                    FROM " + mainExchange(model) + @" mf
                    WHERE ";


                query += ExchangesOneEnabled(model);


                //Console.WriteLine(query);
            }
            else
            {
                if (MainDisabledExchange(model) != "NONE")
                {

                    query += DisabledExchanges(model);
                    //Console.WriteLine("hello");
                    Console.WriteLine("here1" +  query);

                }
                else
                {

                    query = "SELECT s.Symbol, s.BaseAsset, s.QuoteAsset FROM(SELECT Symbol, BaseAsset, QuoteAsset FROM Binance_Futures" +
                            " Union SELECT Symbol, BaseAsset, QuoteAsset FROM Binance_Spot" +
                            " Union SELECT Symbol, BaseAsset, QuoteAsset FROM Bybit_Futures" +
                            " Union  SELECT Symbol, BaseAsset, QuoteAsset FROM Bybit_Spot " +
                            " Union SELECT Symbol, BaseAsset, QuoteAsset FROM Bitget_Futures " +
                            " Union SELECT Symbol, BaseAsset, QuoteAsset FROM Bitget_Spot" +
                            " Union SELECT Symbol, BaseAsset, QuoteAsset FROM Gate_Futures" +
                            " Union SELECT Symbol, BaseAsset, QuoteAsset FROM Gate_Spot" +
                            " Union SELECT Symbol, BaseAsset, QuoteAsset FROM Mexc_Futures" +
                            " Union SELECT Symbol, BaseAsset, QuoteAsset FROM Mexc_Spot) AS s;";
                    Console.WriteLine("here2" + query);

                }
            }

            List<string> coins = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    coins.Add($"{reader["Symbol"]} ({reader["BaseAsset"]}/{reader["QuoteAsset"]})");
                }
            }

            model.FilteredCoins = coins;

            return View("Index", model);
        }

    }
}
