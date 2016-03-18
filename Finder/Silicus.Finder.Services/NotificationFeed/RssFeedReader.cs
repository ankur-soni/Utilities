#region Using

using System;
using System.Web.Caching;
using System.Xml;
using System.Collections.ObjectModel;
using System.Web;


#endregion

namespace ConsoleApplication1
{
    /// <summary>
    /// Parses notification RSS 2.0 feeds.
    /// </summary>
    [Serializable]
    public class RssReader : IDisposable
    {
        #region Constructors

        public RssReader(string feedUrl)
        {
            _feedUrl = feedUrl;
        }

        #endregion

        #region Properties

        private string _feedUrl;

        private readonly Collection<RssItem> _items = new Collection<RssItem>();
        /// <summary>
        /// Gets all the items in the RSS feed.
        /// </summary>
        public Collection<RssItem> Items
        {
            get { return _items; }
        }

        private string _Title;
        /// <summary>
        /// Gets the title of the RSS feed.
        /// </summary>
        public string Title
        {
            get { return _Title; }
        }

        private string _Description;
        /// <summary>
        /// Gets the description of the RSS feed.
        /// </summary>
        public string Description
        {
            get { return _Description; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the remote RSS feed and parses it.
        /// </summary>
        /// <exception cref="System.Net.WebException" />
        public Collection<RssItem> Execute()
        {
            if (String.IsNullOrEmpty(_feedUrl))
                throw new ArgumentException("The feed url must be set");

            using (XmlReader reader = XmlReader.Create(_feedUrl))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);

                ParseElement(doc.SelectSingleNode("//channel"), "title", ref _Title);
                ParseElement(doc.SelectSingleNode("//channel"), "description", ref _Description);
                ParseItems(doc);

                return _items;
            }
        }

        /// <summary>
        /// Parses the xml document in order to retrieve the RSS items.
        /// </summary>
        private void ParseItems(XmlDocument doc)
        {
            _items.Clear();
            XmlNodeList nodes = doc.SelectNodes("rss/channel/item");

            foreach (XmlNode node in nodes)
            {
                RssItem item = new RssItem();
                ParseElement(node, "title", ref item.Title);
                ParseElement(node, "description", ref item.Description);
                ParseElement(node, "link", ref item.Link);

                string date = null;
                ParseElement(node, "pubDate", ref date);
                DateTime.TryParse(date, out item.Date);

                string dateTicks = null;
                ParseElement(node, "expiration", ref dateTicks);
                item.ExpireOn = FromUnixTime(long.Parse(dateTicks));

                _items.Add(item);
            }
        }

        /// <summary>
        /// Parses the XmlNode with the specified XPath query 
        /// and assigns the value to the property parameter.
        /// </summary>
        private void ParseElement(XmlNode parent, string xPath, ref string property)
        {
            XmlNode node = parent.SelectSingleNode(xPath);
            if (node != null)
                property = node.InnerText;
            else
                property = "Unresolvable";
        }

        private DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        #endregion

        #region IDisposable Members

        private bool _IsDisposed;

        /// <summary>
        /// Performs the disposal.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing && !_IsDisposed)
            {
                _items.Clear();
                _feedUrl = null;
                _Title = null;
                _Description = null;
            }

            _IsDisposed = true;
        }

        /// <summary>
        /// Releases the object to the garbage collector
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }

    #region RssItem struct

    /// <summary>
    /// Represents a RSS feed item.
    /// </summary>
    [Serializable]
    public struct RssItem
    {
        /// <summary>
        /// The publishing date.
        /// </summary>
        public DateTime Date;

        /// <summary>
        /// The title of the item.
        /// </summary>
        public string Title;

        /// <summary>
        /// A description of the content or the content itself.
        /// </summary>
        public string Description;

        /// <summary>
        /// The link to the webpage where the item was published.
        /// </summary>
        public string Link;

        public DateTime ExpireOn;
    }

    #endregion
}
