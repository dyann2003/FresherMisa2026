using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Entities
{
    public class PagingRequest
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Search { get; set; }

        public string Sort { get; set; } //vd: +ModifiedDate

        /// <summary>
        /// DepartmentCode;DepartmentName
        /// </summary>
        public string SearchFields { get; set; }
    }

}
