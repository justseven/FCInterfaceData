
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class EasyUIGridModel
    {
        public EasyUIGridModel(int pageIndex, int totalRows, IEnumerable rows)
        {
            Page = pageIndex;
            Total = totalRows;
            Rows = rows;
        }

        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "rows")]
        public IEnumerable Rows { get; set; }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}