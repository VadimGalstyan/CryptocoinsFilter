namespace CryptocoinsFilter.Models
{
    public class Filter
    {
        // Exchanges that can be choosed
        public bool? BinanceSpot { get; set; }
        public bool? BinanceFutures { get; set; }

        public bool? BybitSpot { get; set; }
        public bool? BybitFutures { get; set; }

        public bool? BitgetSpot { get; set; }
        public bool? BitgetFutures { get; set; }

        public bool?  GateSpot { get; set; }
        public bool? GateFutures { get; set; }

        public bool? MexcSpot { get; set; }
        public bool? MexcFutures { get; set; }

        //NOT SUPPORTED EXCHANGES///
        public bool KucoinSpot { get; set; }
        public bool KucoinFutures { get; set; }

        public bool BingxSpot { get; set; }
        public bool BingxFutures { get; set; }

        public bool LbankSpot { get; set; }
        public bool LbankFutures { get; set; }

        public bool HtxSpot { get; set; }
        public bool HtxFutures { get; set; }

        public bool OkxSpot { get; set; }
        public bool OkxFutures { get; set; }

        public bool HyperliquidSpot { get; set; }
        public bool HyperliquidFutures { get; set; }

        public bool BitmartSpot { get; set; }
        public bool BitmartFutures { get; set; }

        // Result list
        public List<string> FilteredCoins { get; set; } = new List<string>();
    }
}
