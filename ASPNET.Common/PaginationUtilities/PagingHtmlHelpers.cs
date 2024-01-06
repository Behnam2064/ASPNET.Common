using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.PaginationUtilities
{
    public static class PagingHtmlHelpers
    {
        public static IHtmlContent PageLinks
            (this IHtmlHelper htmlHelper, PageInfo pageInfo, Func<int, string> PageUrl)
        {


            StringBuilder pagingTags = new StringBuilder();

            #region Root html tag
            /*
                        if (!string.IsNullOrEmpty(pageInfo.TagRootItems))
                        {
                            pagingTags.Append($"<{pageInfo.TagRootItems}");
                            if (!string.IsNullOrEmpty(pageInfo.ClassRootItems))
                            {
                                pagingTags.Append($" class=\"{pageInfo.ClassRootItems}\"");
                            }
                        }
            */
            #endregion

            //Prev Page
            if (pageInfo.CurrentPage > 1)
            {
                pagingTags.Append(GetTagString
                                 ("Prev", PageUrl(pageInfo.CurrentPage - 1), false, pageInfo));
            }

            if (pageInfo.MaxNumber < 1)
            {
                //Default way
                //Page Numbers
                for (int i = 1; i <= pageInfo.LastPage; i++)
                {
                    pagingTags.Append(GetTagString(i.ToString(), PageUrl(i), i == pageInfo.CurrentPage, pageInfo));
                }

            }
            else
            {
                //New way
                //Default way

                int centerPage = pageInfo.MaxNumber / 2;
                /*if (pageInfo.MaxNumberShow / 2 > 1)
                    centerPage++;*/
                int ForwardPage = pageInfo.CurrentPage + centerPage;
                int BackwardPage = pageInfo.CurrentPage - centerPage;


                if (BackwardPage < 1)
                {
                    BackwardPage = 1;
                    ForwardPage = pageInfo.MaxNumber;

                }
                else
                {
                    if (BackwardPage > 1)
                    {
                        if (ForwardPage > pageInfo.LastPage)
                        {
                            BackwardPage -= ForwardPage - pageInfo.LastPage;
                            if (BackwardPage < 1)
                                BackwardPage = 1;
                            else
                                pagingTags.Append($"<span class=\"{pageInfo.ClassItems}\">...</span>");

                        }
                        else
                        {
                            pagingTags.Append($"<span class=\"{pageInfo.ClassItems}\">...</span>");

                        }

                    }
                }

                int EndBackwardPage = BackwardPage + centerPage;

                if (EndBackwardPage > pageInfo.LastPage)
                    if (pageInfo.LastPage == 1)
                        EndBackwardPage = pageInfo.LastPage;
                    else
                        EndBackwardPage = pageInfo.LastPage + 1;





                for (int i = BackwardPage; i < EndBackwardPage; i++)
                {
                    pagingTags.Append(GetTagString(i.ToString(), PageUrl(i), i == pageInfo.CurrentPage, pageInfo));
                }






                if (ForwardPage > pageInfo.LastPage)
                {
                    ForwardPage -= ForwardPage - pageInfo.LastPage;
                }



                for (int i = BackwardPage + centerPage; i <= ForwardPage; i++)
                {
                    pagingTags.Append(GetTagString(i.ToString(), PageUrl(i), i == pageInfo.CurrentPage, pageInfo));
                }


                if (ForwardPage < pageInfo.LastPage)
                {
                    pagingTags.Append($"<span class=\"{pageInfo.ClassItems}\">...</span>");
                }




                /*for (int i = 1; i <= pageInfo.LastPage; i++)
                {
                    pagingTags.Append(GetTagString(i.ToString(), PageUrl(i)));
                }*/

            }
            //Next Page
            if (pageInfo.CurrentPage < pageInfo.LastPage)
            {
                if (pageInfo.LastPage > 1)
                    pagingTags.Append(GetTagString
                                     ("Next", PageUrl(pageInfo.CurrentPage + 1), false, pageInfo));
            }


            #region Root html tag

            /* if (!string.IsNullOrEmpty(pageInfo.TagRootItems))
             {
                 pagingTags.Append($"</{pageInfo.TagRootItems}>");
             }*/

            #endregion


            //paging tags
            return new HtmlString(pagingTags.ToString());
        }

        private static string GetTagString(
            string innerHtml, string hrefValue, bool IsCurrentPage, PageInfo pageInfo)
        {
            TagBuilder tag = new TagBuilder(IsCurrentPage ? pageInfo.TagItemCurrentPage : pageInfo.TagItems); // Construct an <a> tag
            if (!IsCurrentPage)
            {
                if (!string.IsNullOrEmpty(pageInfo.ClassItems))
                    tag.MergeAttribute("class", pageInfo.ClassItems);
                tag.MergeAttribute("href", hrefValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(pageInfo.ClassItemCurrentPage))
                    tag.MergeAttribute("class", pageInfo.ClassItemCurrentPage);

            }
            tag.InnerHtml.Append(" " + innerHtml + "  ");
            using (var sw = new System.IO.StringWriter())
            {
                tag.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
                return sw.ToString();
            }
        }
    }
}
