using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace XZFCDA.FC.Models
{
    public class Result
    {
        /// <summary>
        /// 条目
        /// </summary>
        public VolEleArc VolEleArc { set; get; }
        /// <summary>
        /// 条目对应的附件集合
        /// </summary>
        public List<VolEleArcDtl> VolEleArc_List { get; set; }
    }
}
