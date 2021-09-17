using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace UnsplashAPI {
    public partial class Form1 : Form {
        
        public Form1() {
            InitializeComponent();
        }

        /// <summary>
        /// get method
        /// </summary>
        /// <param name="contentType">request string</param>
        /// <returns></returns>
        private async Task<string> MakeRequestAsync(string contentType) {
            string data = string.Empty;
            string url = "https://api.unsplash.com/search/photos";
            string fullUrl = url + "?query=" + contentType + "&client_id=Z70rmFmymj3V-QvQxvNv_ZWo8e8dZjIvdSTZAqCIxUM";

            using (var client = new HttpClient()) {
                client.Timeout = TimeSpan.FromSeconds(3);
                data = await client.GetStringAsync(fullUrl);
            }

            return data;
        }

        /// <summary>
        /// fill browser content with pictures
        /// </summary>
        /// <param name="html">html string content</param>
        private void DisplayHtml(string html) {
            webBrowser1.Navigate("about:blank");
            
            if (webBrowser1.Document != null) {
                webBrowser1.Document.Write(string.Empty);
            }

            webBrowser1.DocumentText = html;
        }


        // on get request button clicked
        private async void button1_Click(object sender, EventArgs e) {
            string queryText = textBox1.Text.Trim();
            
            if (queryText != "") {
                label1.Visible      = false;
                textBox1.Visible    = false;
                button1.Visible     = false;

                var data = await MakeRequestAsync(queryText);

                dynamic dataObject = JsonConvert.DeserializeObject(data);

                var html = string.Empty;
                var requestResults = dataObject.results;
                foreach (var item in requestResults) {
                    html += "<img src=\"" + item.urls.small + "\"><br><br>";

                    Console.WriteLine("Picture : " + item.urls.small);

                    DisplayHtml(html);
                }

                button2.Visible     = true;
                webBrowser1.Visible = true;
            }
        }

        // on back button clicked
        private void button2_Click(object sender, EventArgs e) {
            button2.Visible     = false;
            webBrowser1.Visible = false;
            label1.Visible      = true;
            textBox1.Visible    = true;
            button1.Visible     = true;
        }
    }
}
