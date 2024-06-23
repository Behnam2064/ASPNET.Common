using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.AuthorizeAttributeFilters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ListCountRequiredAttribute : RequiredAttribute
    {
        private readonly uint _minElements;
        public ListCountRequiredAttribute(uint MinCount)
        {
            this._minElements = MinCount;
        }
        public override bool IsValid(object? value)
        {
            if (!base.IsValid(value))
                return false;
            else
            {
                IList? list = value as IList;

                return list != null && list.Count > _minElements;
            }
        }
    }
}
