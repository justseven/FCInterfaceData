using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.TAXInterface
{
    public class TaxLogModel
    {
        public string SQBH { get; set; }
        public string HTBAH { get; set; }
        public string Json { get; set; }
        public string IsSuccess { get; set; }

        public string Message { get; set; }

        public DateTime CreateTime { get; set; }
    }
}