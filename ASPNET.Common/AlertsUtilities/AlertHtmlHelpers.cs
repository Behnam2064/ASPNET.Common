using ASPNET.Common.PaginationUtilities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.AlertsUtilities
{
    public enum AlertBootstrapType
    {
        primary,
        secondary,
        success,
        warning,
        danger,
        info,
        light,
        dark
    }

    public static class AlertHtmlHelpers
    {

        public static IHtmlContent AlertBootstrap(this IHtmlHelper htmlHelper, object Content, AlertBootstrapType alertType)
        {
            return new HtmlString($"""
                <div class="alert alert-{alertType}" role="alert">
                {Content}
                </div>
                """);
        }

        public static IHtmlContent AlertDismissibleBootstrap(this IHtmlHelper htmlHelper, object Content, AlertBootstrapType alertType)
        {

            return new HtmlString($"""
                <div class="alert alert-{alertType} alert-dismissible" role="alert">
                {Content}
                  <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
                """);
        }


        /// <summary>
        /// How to use it?
        /// ASP .NET
        ///  ViewBag.AlertBootstrapAutoInfo = "Information 1";
        ///  ViewBag.AlertBootstrapAutoInfo2 = "Information 2";
        ///  ViewBag.AlertBootstrapAutoWarning = "Warning 1";
        ///  
        /// or 
        ///  ViewBag.AlertBootstrapAutoWarning_2 = "Warning 2";
        /// 
        /// 
        /// Razor pages
        /// 
        /// @foreach (var element in Html.AlertBootstrapAuto(ViewData, stringComparison: StringComparison.InvariantCultureIgnoreCase))
        /// @element
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="appData"></param>
        /// <param name="Prefix"></param>
        /// <param name="stringComparison"></param>
        /// <param name="IsDismissible"></param>
        /// <returns></returns>
        public static IEnumerable<IHtmlContent> AlertBootstrapAuto<TModel>(this IHtmlHelper htmlHelper, ViewDataDictionary<TModel> appData, string Prefix = nameof(AlertBootstrapAuto), StringComparison stringComparison = StringComparison.Ordinal, bool IsDismissible = true)
        {
            IList<IHtmlContent> htmlTags = new List<IHtmlContent>();
            if (appData == null)
                return htmlTags;


            Func<IHtmlHelper, object, AlertBootstrapType, IHtmlContent> alertMakerMethod1 = AlertDismissibleBootstrap;
            if (!IsDismissible)
                alertMakerMethod1 = AlertBootstrap;

            var names = Enum.GetValues<AlertBootstrapType>();
            //dynamic ViewBag =(decimal)viewBag;

            foreach (var item in names)
            {
                try
                {
                    if (appData.Any(x => x.Key.StartsWith(Prefix + item, stringComparison)))
                    {
                        foreach (var item2 in appData.Where(x => x.Key.StartsWith(Prefix + item, stringComparison)).ToList())
                        {
                            htmlTags.Add(alertMakerMethod1.Invoke(htmlHelper, item2.Value as string, item));

                        }
                    }
                }
                catch { }

            }

            return htmlTags;
        }
    }
}
