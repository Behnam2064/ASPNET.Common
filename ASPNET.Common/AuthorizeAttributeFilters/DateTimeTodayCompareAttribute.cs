using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.AuthorizeAttributeFilters
{
    public enum DateTimeCompareType
    {
        Greater,
        GreaterThanOrEqual,
        Equal,
        LessThen,
        LessThenOrEqaul,
        NotEqual,
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateTimeCompareAttribute : ValidationAttribute
    {
        private readonly DateTimeCompareType compaterType;
        public readonly DateTime dateTime;
        private readonly bool AllowNull;
        public DateTimeCompareAttribute(DateTimeCompareType compaterType, bool AllowNull, double AddDays = 0, double AddHours = 0, double AddMinutes = 0, double AddSeconds = 0, double AddMilliseconds = 0)
        {
            this.compaterType = compaterType;
            this.dateTime = DateTime.Today.AddDays(AddDays).AddHours(AddHours).AddMinutes(AddMinutes).AddSeconds(AddSeconds).AddMilliseconds(AddMilliseconds); //dateTime;
            this.AllowNull = AllowNull;
        }

        public override bool IsValid(object? value)
        {
            if (AllowNull && value is null)
            {
                return true;
            }
            else
            {
                if (value is not DateTime)
                    return false;

                DateTime? date = (DateTime)value;

                switch (compaterType)
                {
                    case DateTimeCompareType.Greater:
                        return date > dateTime;
                    case DateTimeCompareType.GreaterThanOrEqual:
                        return date >= dateTime;
                    case DateTimeCompareType.Equal:
                        return date == dateTime;
                    case DateTimeCompareType.LessThen:
                        return date < dateTime;
                    case DateTimeCompareType.LessThenOrEqaul:
                        return date <= dateTime;
                    case DateTimeCompareType.NotEqual:
                        return date != dateTime;
                    default:
                        return false;
                }
            }
        }
    }




    public class DateTimeTodayCompareAttribute : DateTimeCompareAttribute
    {
        private readonly DateTimeCompareType compaterType;
        public virtual DateTime dateTime { get; set; }
        private readonly bool AllowNull;
        public DateTimeTodayCompareAttribute(DateTimeCompareType compaterType, bool AllowNull) : base(compaterType, AllowNull)
        {
        }

    }

    public class DateTimeNowCompareAttribute : DateTimeCompareAttribute
    {
        public DateTimeNowCompareAttribute(DateTimeCompareType compaterType, bool AllowNull) : base(compaterType, AllowNull, AddHours: DateTime.Now.Hour, AddMinutes: DateTime.Now.Minute, AddMilliseconds: DateTime.Now.Millisecond)
        {
        }
    }

    public class DateTimeTomorrowCompareAttribute : DateTimeCompareAttribute
    {
        public DateTimeTomorrowCompareAttribute(DateTimeCompareType compaterType, bool AllowNull) : base(compaterType, AllowNull, AddDays:1, AddHours: DateTime.Now.Hour, AddMinutes: DateTime.Now.Minute, AddMilliseconds: DateTime.Now.Millisecond)
        {
        }
    }

    public class DateTimeYesterdayCompareAttribute : DateTimeCompareAttribute
    {
        public DateTimeYesterdayCompareAttribute(DateTimeCompareType compaterType, bool AllowNull) : base(compaterType, AllowNull, AddDays: -1, AddHours: DateTime.Now.Hour, AddMinutes: DateTime.Now.Minute, AddMilliseconds: DateTime.Now.Millisecond)
        {
        }
    }
}
