using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.PaginationUtilities
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int MaxNumberShow { get; set; }

        /// <summary>
        /// Like page-item , page-link , and ...
        /// </summary>
        public string? ClassItems { get; set; }

        /// <summary>
        /// Like page-item , page-link , and ...
        /// </summary>
        public string? ClassItemCurrentPage { get; set; }

        /// <summary>
        /// Like span , li and ...
        /// </summary>
        public string TagItemCurrentPage { get; set; }
        public string TagItems { get; set; }
/*
        public string TagRootItems { get; set; }
        /// <summary>
        /// like pagination
        /// https://getbootstrap.com/docs/5.3/components/pagination/#overview
        /// </summary>
        public string? ClassRootItems { get; set; }*/

        public PageInfo()
        {
            CurrentPage = 1;
            TagItemCurrentPage = "span";
            TagItems = "li";
            /*TagRootItems = "ul";
            ClassRootItems = "pagination";*/

        }
        //starting item number in the page
        public int PageStart
        {
            get { return ((CurrentPage - 1) * ItemsPerPage + 1); }
        }
        //last item number in the page
        public int PageEnd
        {
            get
            {
                int currentTotal = (CurrentPage - 1) * ItemsPerPage + ItemsPerPage;
                return (currentTotal < TotalItems ? currentTotal : TotalItems);
            }
        }
        public int LastPage
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}
