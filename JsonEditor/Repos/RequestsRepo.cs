using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Web;

namespace ZbigniewJson.Repos
{
    public static class RequestsRepo
    {
        public const string GoogleTranslateUrl = "https://translation.googleapis.com/language/translate/v2";
        private static readonly HttpClient client = new HttpClient();

        public async static Task<string> GoogleTranslate(string textToTranslate,string sourceLangCode, string destLangCode)
        {
            if(!string.IsNullOrEmpty(textToTranslate) && !string.IsNullOrEmpty(sourceLangCode) && !string.IsNullOrEmpty(destLangCode))
            {
                string translatedText;
                var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLangCode}&tl={destLangCode}&dt=t&q={HttpUtility.UrlEncode(textToTranslate)}";
                var webClient = new WebClient
                {
                    Encoding = Encoding.UTF8
                };
                var result = webClient.DownloadString(url);
                try
                {
                    translatedText = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                    return translatedText;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Something went wrong with request. " +ex, "Error", MessageBoxButton.OK);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Not enough data to translate", "Error", MessageBoxButton.OK);
                return null;
            }
        }
    }
}
