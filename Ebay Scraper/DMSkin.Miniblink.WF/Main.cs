using Kyozy.MiniblinkNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace DMSkin.Miniblink.WF
{
    public partial class Main : Form
    {
        private WebView m_ebayview;
        public Main()
        {
            InitializeComponent();

            m_ebayview = new WebView(); // Create a new WebView
            m_ebayview.Bind(ebay_browser); // Bind to the Parent
            url_textbox.Text = "http://www.ebay.com";
            m_ebayview.OnLoadingFinish += loading_Finished;
            btn_save.Text = "Loading Website...";
            btn_save.Enabled = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            m_ebayview.LoadURL("http://www.ebay.com");
        }

        private void url_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                Navigate(url_textbox.Text);
            }
        }

        private void loading_Finished(object sender, LoadingFinishEventArgs e)
        {
            btn_save.Text = "Save items into .csv file...";
            btn_save.Enabled = true;
        }

        private void Navigate(String url) {
            btn_save.Text = "Loading Website...";
            btn_save.Enabled = false;
            if (String.IsNullOrEmpty(url) || url_textbox.Text.Equals("about:blank")) {
                MessageBox.Show("Enter a valid URL.");
                url_textbox.Focus();
                return;
            }
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "http://" + url;
            try
            {
                m_ebayview.LoadURL(url);
            }
            catch(System.UriFormatException) {
                return;
            }
        }

        private void btn_go_Click(object sender, EventArgs e)
        {
            String url = url_textbox.Text;
            Navigate(url);
        }

        private void url_textbox_Click(object sender, EventArgs e)
        {
            url_textbox.SelectAll();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = ".csv file (*.csv) | *.csv";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK) {
                String filename = dlg.FileName;
                // Open the Progress dialog
                WaitForm frm = new WaitForm(filename, m_ebayview.GetURL());
                frm.ShowDialog();
            }
        }

    }
}
