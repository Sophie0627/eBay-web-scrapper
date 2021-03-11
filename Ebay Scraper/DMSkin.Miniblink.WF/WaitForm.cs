using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace DMSkin.Miniblink.WF
{
    public partial class WaitForm : Form
    {
        String url;
        String filename;
        
        public WaitForm(String _filename, String _url)
        {
            InitializeComponent();
            filename = _filename;
            url = _url;
        }

        private void WaitForm_Load(object sender, EventArgs e)
        {
            pBar.MarqueeAnimationSpeed = 30;
        }

        private async void GetHtmlAsync()
        {
            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);

            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);

            // Get the items information
            // Get the list of items

            var ProductListItems = htmlDocument.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("s-item")).ToList();

            List<string> productUrls = new List<string>();

            foreach (var ProductListItem in ProductListItems)
            {
                var a_tag = ProductListItem.Descendants("a").FirstOrDefault();

                if (a_tag != null)
                {
                    var urls = a_tag.GetAttributeValue("href", "");
                    productUrls.Add(urls);
                }

            }

            StringBuilder sb = new StringBuilder();
            sb.Append("ProductionName,");
            sb.Append("ImageURL,");
            sb.Append("Price,");
            sb.Append("Option");
            sb.AppendLine();

            // Extract item info from each product URL
            foreach (var productURL in productUrls)
            {
                html = await httpclient.GetStringAsync(productURL);
                htmlDocument.LoadHtml(html);
                
                ProductInfo info = new ProductInfo();
                info.ProductInit();

                // Extract Image URL
                var imgsrc = htmlDocument.DocumentNode.Descendants("img")
                    .Where(node => node.GetAttributeValue("id", "")
                    .Equals("icImg")).ToList();
                info.ImgUrl = imgsrc.First().GetAttributeValue("src", "");

                // Extract Production Title
                var ttl = htmlDocument.DocumentNode.Descendants("h1")
                    .Where(node => node.GetAttributeValue("id", "")
                    .Equals("itemTitle")).ToList();
                info.ProductionName = ttl.First().Descendants("span").First().InnerText;

                // Extract Price
                var price = htmlDocument.DocumentNode.Descendants("span")
                    .Where(node => node.GetAttributeValue("itemprop", "")
                    .Equals("price")).ToList();
                info.Price = price.First().InnerHtml;

                // Extract Option
                var options = htmlDocument.DocumentNode.Descendants("select").ToList();
                foreach (var option in options)
                {
                    var opt = option.GetAttributeValue("name", "");
                    info.Option = info.Option + ";" + opt;
                }

                sb.Append(info.ProductionName + ",");
                sb.Append(info.ImgUrl + ",");
                sb.Append(info.Price + ",");
                sb.Append(info.Option);
                sb.AppendLine();
            }

            System.IO.File.WriteAllText(filename, sb.ToString());
            this.Close();
        }

        private void WaitForm_Shown(object sender, EventArgs e)
        {
            GetHtmlAsync();           
        }

        private void WaitForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) {
                this.Close();
            }
        }

    }
}
