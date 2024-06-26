using ASPNET.Common.PaginationUtilities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    }
}
