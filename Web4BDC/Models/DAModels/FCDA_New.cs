using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.FC.Models;
using Web4BDC.Models.BDCModel;

namespace Web4BDC.Models.DAModels
{
    public class FCDA_New
    {
        /// <summary>
        /// 收件单信息
        /// </summary>
        public DJ_SJD SJD_List { get; set; }
        /// <summary>
        /// 业务对应户信息
        /// </summary>
        public List<FC_H_QSDC> H_List { get; set; }
        /// <summary>
        /// 权利人信息
        /// </summary>
        public List<DJ_QLR> QLR_List { get; set; }
        /// <summary>
        /// 证书信息
        /// </summary>
        public List<Certificate> Cer_List { get; set; }
        /// <summary>
        /// 附件信息
        /// </summary>
        public List<WFM_ATTACHLST> AttachList { get; set; }
    }
}