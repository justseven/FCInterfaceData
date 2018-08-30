using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Easyui
{
    public class SortProperty
    {
        public SortProperty()
        {
            NullsLast = true;
        }

        public string PropertyName { get; set; }
        public SortType SortType { get; set; }
        public bool NullsLast { get; set; }
    }


    public enum SortType : int
    {
        Default = 0,

        Asc = 1,

        Desc = 2
    }
}