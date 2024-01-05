using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.PaginationUtilities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Metrics;
    using System.Numerics;

    namespace HTMLHelpersApp.Models
    {
        public class InputNumber
        {
            //validation for required, only numbers, allowed range-1 to 500
            [Required(ErrorMessage = "Value is Required!. Please enter value between 1 and 500.")]
            [RegularExpression(@"^\d+$",
            ErrorMessage = "Only numbers are allowed. Please enter value between 1 and 500.")]
            [Range(1, 500, ErrorMessage = "Please enter value between 1 and 500.")]
            public int Number = 1;
        }
    }
}
