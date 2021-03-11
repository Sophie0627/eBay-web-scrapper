using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSkin.Miniblink.WF
{
    public class ProductInfo
    {
        public void ProductInit() {
            Id = 0;
            ImgUrl = "";
            ProductionName = "";
            Price = "";
            Option = "";
        }
        public int Id { get; set; }

        public string ImgUrl { get; set; }

        public string ProductionName { get; set; }

        public string Price { get; set; }

        public string Option { get; set; }
        /*
        public string ProductName { get; set; }
        public string ListingTitle { get; set; }
        public string Condition { get; set; }
        public double CurrentBid { get; set; }
        public double ShippingPrice { get; set; }
        public string Url { get; set; }
        public long ISBN { get; set; }
        public DateTime TimeLeft { get; set; }

        public double TotalPrice => ShippingPrice + CurrentBid;
        */
    }
}
