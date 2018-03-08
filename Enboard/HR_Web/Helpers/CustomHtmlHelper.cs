using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_Web.Helpers
{
    public class HtmlAttribute
    {
        public string @class { get; set; }
    }

    public static class CustomHtmlHelper
    {

        public static IHtmlString DropDownListFor(int? value, SelectList list, string classNames)
        {

            string valueStr = value != null ? value.ToString() : string.Empty;
            var selectedOption = list.FirstOrDefault(t => t.Value == valueStr);
            var inputText = selectedOption != null ? selectedOption.Text : string.Empty;
            string htmlString = string.Format("<input  value='{0}' class='{1}' />", inputText, classNames);
            return new HtmlString(htmlString);
        }

        public static IHtmlString DropDownListFor(string value, SelectList list, string classNames)
        {

            var selectedOption = list.FirstOrDefault(t => t.Value == value);
            var inputText = selectedOption != null ? selectedOption.Text : string.Empty;

            string htmlString = string.Format("<input  value='{0}' class='{1}' />", inputText, classNames);
            return new HtmlString(htmlString);
        }

        public static IHtmlString DropDownListForWithOtherOption(int? value, SelectList list, string otherOption, string classNames)
        {
            string htmlString = string.Empty;
            IHtmlString IhtmlString;
            if (!string.IsNullOrWhiteSpace(otherOption))
            {
                htmlString = string.Format("<input  value='{0}' class='{1}' />", otherOption, classNames);
                IhtmlString = new HtmlString(htmlString);
            }
            else
            {
                IhtmlString = DropDownListFor(value, list, classNames);
            }

            return IhtmlString;
        }

        public static IHtmlString BirthPlace(string value, SelectList cityList, SelectList stateList, string otherOptionValue, string classNames)
        {
            string inputText = string.Empty;
            if (!string.IsNullOrWhiteSpace(otherOptionValue))
            {
                inputText = otherOptionValue;

            }
            else
            {
                if (value != null)
                {
                    string[] tokens = value.Split('-');
                    if (tokens.Length == 2)
                    {
                        var stateId = tokens[0];
                        var cityId = tokens[1];
                        var city = cityList.FirstOrDefault(t => t.Value == cityId);
                        var cityText = city != null ? city.Text : string.Empty;
                        var state = stateList.FirstOrDefault(t => t.Value == stateId);
                        var stateText = state != null ? state.Text : string.Empty;
                        inputText = cityText + " - " + stateText;

                    }
                }


            }
            string htmlString = string.Format("<input  value='{0}' class='{1}' />", inputText, classNames);
            return new HtmlString(htmlString);
        }

    }
}