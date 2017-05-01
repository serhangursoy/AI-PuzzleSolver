using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace iaprojectform
{
    class Parser
    {
        // Might use this for further extention
        public Parser() {}


        public List<string> GetSynonym( string keyword)
        {
            List<string> resultList = new List<string>();

            string url = "http://www.thesaurus.com/browse/" + keyword;
            string HTML_FILE = "";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        HTML_FILE += result;
                    }
                }
            }

            HtmlAgilityPack.HtmlDocument document2 = new HtmlAgilityPack.HtmlDocument();
            document2.LoadHtml(HTML_FILE);

            HtmlNode[] rows = document2.DocumentNode.SelectNodes("//span[@class='text']").ToArray();

            Console.WriteLine("***** Printing all synonyms *****");
            foreach (HtmlNode a in rows)
            {
                resultList.Add(a.InnerText);
                Console.WriteLine("- " + a.InnerText);
            }

            return resultList;
        }


        public List<string> GetDictionary(string keyword)
        {
            
            // def-content for first. 

            List<string> resultList = new List<string>();

            string url = "http://www.dictionary.com/browse/" + keyword;
            string HTML_FILE = "";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        HTML_FILE += result;
                    }
                }
            }

            HtmlAgilityPack.HtmlDocument document2 = new HtmlAgilityPack.HtmlDocument();
            document2.LoadHtml(HTML_FILE);

            HtmlNode[] rows = document2.DocumentNode.SelectNodes("//div[@class='def-content']").ToArray();

            Console.WriteLine("***** Printing all matches from dictionary *****");
            foreach (HtmlNode a in rows)
            {
                string element = a.InnerText.Trim();
                resultList.Add(element);
                Console.WriteLine("+ " + element);
            }

            return resultList;
        }


        public Dictionary<string,int> GetFromGoogle(string keyword)
        {

            // def-content for first. 
            Dictionary<string, int> resultDic = new Dictionary<string, int>();

            List<string> tempList = new List<string>();

            string url = "https://www.google.com.tr/search?site=&source=hp&q=" + keyword;
            string HTML_FILE = "";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        HTML_FILE += result;
                    }
                }
            }

            HtmlAgilityPack.HtmlDocument document2 = new HtmlAgilityPack.HtmlDocument();
            document2.LoadHtml(HTML_FILE);

            HtmlNode[] descriptionText = document2.DocumentNode.SelectNodes("//span[@class='st']").ToArray();
            HtmlNode[] headerText = document2.DocumentNode.SelectNodes("//h3[@class='r']").ToArray();

            Console.WriteLine("***** Printing all matches from Google *****");
            foreach (HtmlNode a in descriptionText) {
                a.RemoveChild(a.FirstChild, true);
                string element = a.InnerText.Trim();
                tempList.Add(element);
                Console.WriteLine("% " + element);
            }


            foreach (HtmlNode b in headerText) {
                tempList.Add(b.InnerText);
                Console.WriteLine("# " + b.InnerText);
            }

            List<string> allWords = new List<string>();

            foreach(string elementStr in tempList) {
                string tmp = elementStr.Replace(".", "");
                tmp = tmp.Replace(",", "");
                tmp = tmp.Replace("?", "");
                tmp = tmp.Replace("!", "");
                tmp = tmp.Replace("\n", "");
                tmp = tmp.Replace("...", "");
                tmp = tmp.Replace("\t", "");
                tmp = tmp.Replace("-", "");
                tmp = tmp.Replace(":", "");
                tmp = tmp.Replace("(", "");
                tmp = tmp.Replace(")", "");
                tmp = tmp.Replace("~", "");
                tmp = tmp.Replace("=", "");
                tmp = tmp.Replace("/", "");
                tmp = tmp.Replace("^", "");
                tmp = tmp.Replace("|", "");
                tmp = tmp.Replace("<", "");
                tmp = tmp.Replace(">", "");
                tmp = tmp.Replace("'", "");
                tmp = tmp.Replace("\"", "");
                tmp = tmp.Replace("\b", "");
                tmp = tmp.Trim();
                string[] tokens = tmp.Split(' ');
                foreach (string tokenStr in tokens)
                    allWords.Add(tokenStr);
            }


            foreach (string elementStr in allWords)  {
                if (resultDic.ContainsKey(elementStr.ToLower())) {  // If we have that already.
                    resultDic[elementStr.ToLower()] += 1;
                } else {
                    resultDic.Add(elementStr.ToLower(), 1);
                }
            }

            var items = from pair in resultDic
                        orderby pair.Value descending
                        select pair;
            resultDic = items.ToDictionary(t => t.Key, t => t.Value);
            return resultDic;
        }
    }
}
