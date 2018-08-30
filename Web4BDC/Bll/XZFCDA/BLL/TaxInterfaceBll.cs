using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Web4BDC.Dal;
using Web4BDC.TaxService;

namespace Web4BDC.Bll
{
    public class TaxInterfaceBll
    {
        private XmlDocument GetSaveTaxInterfaceData(string SLBH) {
            TaxInterfaceDal dal = new TaxInterfaceDal();
            string conetent= dal.GetTaxInterfaceBySLBH(SLBH);
            if (string.IsNullOrEmpty(conetent))
            {
                return null;
            }
            else {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(conetent);
                return doc;
            }
        }

        private string GetSaveTXMBySlbh(string SLBH) {
            TaxInterfaceDal dal = new TaxInterfaceDal();
            string txm = dal.GetTXMBySLBH(SLBH);
            return txm;
        }
        private XmlDocument GetTaxInterfaceData(string txm, string ry_id,string slbh) {
            if (!string.IsNullOrEmpty(txm) && !string.IsNullOrEmpty(ry_id))
            {
                TaxInterfaceDal dal = new TaxInterfaceDal();
                /********************************************************
                 * 铜山
                 * *****************************************************/

                //DSServer.DSWebService.qsxxWebservicePortTypeClient ss = new DSServer.DSWebService.qsxxWebservicePortTypeClient();
                //string sendXML = string.Format(@"<taxXML>
                //<houseList>
                //<houseVo>
                //<txm>{0}</txm>
                //<ry_id>{1}</ry_id>
                //</houseVo>
                //</houseList>
                //</taxXML>
                //", txm, ry_id);
                //string xmlS = ss.getHouseQs(sendXML);
                /************************************************************
                 *  徐州
                 * *********************************************************/
                ServiceSoap ss = new ServiceSoapClient();

                string xmlS = ss.zhuanfa_qsxx(txm, ry_id);
                
                if (!dal.IfExistsSLBH(slbh))
                    dal.InsertTaxInfo(xmlS, slbh, ry_id, txm);
                else
                    dal.UpdateTaxInfo(xmlS, slbh, ry_id, txm);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlS);
                return doc;
            }
            else {
                throw new Exception("条形码或用户id为空");
            }
        }

        private TAXXML GetModelFromXML(Type type, XmlDocument xml) {
            TAXXML tax = new TAXXML();
           
            XmlNode houseListNode = xml.SelectSingleNode("taxXML/houseList");
            if (houseListNode != null) {
                tax.houseList = new HouseList();
               XmlNode bzVONode= houseListNode.SelectSingleNode("bzVO");
                if (bzVONode != null) {
                    XmlNode bz_bjNode = bzVONode.SelectSingleNode("bz_bj");
                    if (!bz_bjNode.InnerText.Equals("0")) {
                        tax.houseList.BZZM = new Tax_BZZM();
                        tax.houseList.BZZM.BZ_BJ = bzVONode.SelectSingleNode("bz_bj").InnerText;
                        tax.houseList.BZZM.BZ_BZXX = bzVONode.SelectSingleNode("bz_bzxx").InnerText;
                        tax.houseList.BZZM.BZ_DH = bzVONode.SelectSingleNode("bz_dh").InnerText;
                        tax.houseList.BZZM.BZ_DZ = bzVONode.SelectSingleNode("bz_dz").InnerText;
                        tax.houseList.BZZM.BZ_LX = bzVONode.SelectSingleNode("bz_lx").InnerText;
                        tax.houseList.BZZM.BZ_MC = bzVONode.SelectSingleNode("bz_mc").InnerText;
                        tax.houseList.BZZM.BZ_MJ = bzVONode.SelectSingleNode("bz_mj").InnerText;
                        tax.houseList.BZZM.BZ_SJ = bzVONode.SelectSingleNode("bz_sj").InnerText;
                        tax.houseList.BZZM.BZ_SLR = bzVONode.SelectSingleNode("bz_slr").InnerText;
                        tax.houseList.BZZM.BZ_WSH = bzVONode.SelectSingleNode("bz_wsh").InnerText;
                        tax.houseList.BZZM.BZ_YCQR = bzVONode.SelectSingleNode("bz_ycqr").InnerText;
                        tax.houseList.BZZM.BZ_YY= bzVONode.SelectSingleNode("bz_yy").InnerText;
                        
                    }
                }
                XmlNode mzVONode= houseListNode.SelectSingleNode("mzVO");
                if (mzVONode != null) {
                    XmlNode mz_bjNode = mzVONode.SelectSingleNode("mz_bj");
                    if (!mz_bjNode.InnerText.Equals("0")) {
                        tax.houseList.MZZM = new Tax_MZZM();
                        tax.houseList.MZZM.MZ_BJ = mzVONode.SelectSingleNode("mz_bj").InnerText;
                        tax.houseList.MZZM.MZ_CJJG = mzVONode.SelectSingleNode("mz_cjjg").InnerText;
                        tax.houseList.MZZM.MZ_DYSJ = mzVONode.SelectSingleNode("mz_dysj").InnerText;
                        tax.houseList.MZZM.MZ_DZ = mzVONode.SelectSingleNode("mz_dz").InnerText;
                        tax.houseList.MZZM.MZ_JMLX = mzVONode.SelectSingleNode("mz_jmlx").InnerText;
                        tax.houseList.MZZM.MZ_JSJG = mzVONode.SelectSingleNode("mz_jsjg").InnerText;
                        tax.houseList.MZZM.MZ_JZJG = mzVONode.SelectSingleNode("mz_jzjg").InnerText;
                        tax.houseList.MZZM.MZ_MJ = mzVONode.SelectSingleNode("mz_mj").InnerText;
                        tax.houseList.MZZM.MZ_QSJE = mzVONode.SelectSingleNode("mz_qsje").InnerText;
                        tax.houseList.MZZM.MZ_SZJE = mzVONode.SelectSingleNode("mz_szje").InnerText;
                        tax.houseList.MZZM.MZ_WSH = mzVONode.SelectSingleNode("mz_wsh").InnerText;
                        tax.houseList.MZZM.MZ_ZYLX = mzVONode.SelectSingleNode("mz_zylx").InnerText;
                        tax.houseList.MZZM.MZ_ZYSJ = mzVONode.SelectSingleNode("mz_zysj").InnerText;
                       
                    }
                }
                XmlNode qsVONode = houseListNode.SelectSingleNode("qsVO");
                if (qsVONode != null) {
                    tax.houseList.QSXXs = new List<Tax_QSXX>();
                    XmlNodeList qsVONodeList = qsVONode.ChildNodes;
                    Tax_QSXX qsxx = null;
                    foreach (XmlNode qsChildVONode in qsVONodeList) {
                        if (qsChildVONode.Name.Equals("qs_bj"))
                        {
                            if (!qsChildVONode.InnerText.Equals("0"))
                            {
                                qsxx = new Tax_QSXX();
                                tax.houseList.QSXXs.Add(qsxx);
                            }
                            else
                            {
                                break;
                            }
                            qsxx.QS_BJ = qsChildVONode.InnerText;
                        }
                        else {
                            if (qsChildVONode.Name.Equals("qs_fzsj")) {
                                qsxx.QS_FZSJ = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_xqh"))
                            {
                                qsxx.QS_XQH = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_nsrsbm"))
                            {
                                qsxx.QS_NSRSBM = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_mc"))
                            {
                                qsxx.QS_MC = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_dz"))
                            {
                                qsxx.QS_DZ = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_mj"))
                            {
                                qsxx.QS_MJ = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_yt"))
                            {
                                qsxx.QS_YT = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_zsxm"))
                            {
                                qsxx.QS_ZSXM = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_zspm"))
                            {
                                qsxx.QS_ZSPM = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_jsyj"))
                            {
                                qsxx.QS_JSYJ = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_sl"))
                            {
                                qsxx.QS_SL = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_jm"))
                            {
                                qsxx.QS_JM = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_ssqq"))
                            {
                                qsxx.QS_SSQQ = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_ssqz"))
                            {
                                qsxx.QS_SSQZ = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_sjje"))
                            {
                                qsxx.QS_SJJE = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_tpr"))
                            {
                                qsxx.QS_TPR = qsChildVONode.InnerText;
                            }
                            if (qsChildVONode.Name.Equals("qs_bz"))
                            {
                                qsxx.QS_BZ = qsChildVONode.InnerText;
                            }
                        }
                        
                    }
                }
            } 
            return tax;
        }

        public TAXXML GetTaxInterfaceModel(string txm, string ry_id,string slbh) {
            XmlDocument xml = GetTaxInterfaceData(txm, ry_id,slbh); 
            TAXXML tax= GetModelFromXML(typeof(TAXXML), xml);
            return tax;
        }
        public TAXXML GetTaxInterfaceModelBySLBH(string slbh,out string txm) {
            XmlDocument xml = GetSaveTaxInterfaceData(slbh);
            txm = GetSaveTXMBySlbh(slbh);
            if (xml != null)
            {
                TAXXML tax = GetModelFromXML(typeof(TAXXML), xml);
                return tax;
            }
            else {
                return null;
            }
        }
    }
    [XmlRoot(ElementName = "taxXML")]
    public class TAXXML {
        [XmlElement(ElementName = "houseList")]
        public HouseList houseList { get; set; }
    }

    public class HouseList {
        [XmlElement(ElementName = "bzVO")]
        public Tax_BZZM BZZM { get; set; }

        [XmlElement(ElementName = "mzVO")]
        public Tax_MZZM MZZM { get; set; }

        [XmlElement(ElementName = "qsVO")]
        public IList<Tax_QSXX> QSXXs { get; set; }
    }
    /// <summary>
    /// 地税不征证明
    /// </summary>
    public class Tax_BZZM {
        [XmlElement(ElementName = "bz_bj")]
        public string BZ_BJ { get; set; }

        [XmlElement(ElementName = "bz_wsh")]
        public string BZ_WSH{ get; set; }

        [XmlElement(ElementName = "bz_sj")]
        public string BZ_SJ { get; set; }

        [XmlElement(ElementName = "bz_mc")]
        public string BZ_MC { get; set; }

        [XmlElement(ElementName = "bz_dh")]
        public string BZ_DH { get; set; }

        [XmlElement(ElementName = "bz_ycqr")]
        public string BZ_YCQR { get; set; }

        [XmlElement(ElementName = "bz_yy")]
        public string BZ_YY { get; set; }

        [XmlElement(ElementName = "bz_lx")]
        public string BZ_LX { get; set; }

        [XmlElement(ElementName = "bz_mj")]
        public string BZ_MJ { get; set; }

        [XmlElement(ElementName = "bz_dz")]
        public string BZ_DZ { get; set; }

        [XmlElement(ElementName = "bz_slr")]
        public string BZ_SLR { get; set; }

        [XmlElement(ElementName = "bz_bzxx")]
        public string BZ_BZXX { get; set; }
        

    }

    public class Tax_MZZM
    {
        [XmlElement(ElementName = "mz_bj")]
        public string MZ_BJ { get; set; }

        [XmlElement(ElementName = "mz_wsh")]
        public string MZ_WSH { get; set; }

        [XmlElement(ElementName = "mz_qsje")]
        public string MZ_QSJE { get; set; }
        [XmlElement(ElementName = "mz_szje")]
        public string MZ_SZJE { get; set; }

        [XmlElement(ElementName = "mz_zylx")]
        public string MZ_ZYLX { get; set; }
        [XmlElement(ElementName = "mz_jmlx")]
        public string MZ_JMLX { get; set; }

        [XmlElement(ElementName = "mz_dz")]
        public string MZ_DZ { get; set; }

        [XmlElement(ElementName = "mz_mj")]
        public string MZ_MJ { get; set; }

        [XmlElement(ElementName = "mz_zysj")]
        public string MZ_ZYSJ { get; set; }

        [XmlElement(ElementName = "mz_cjjg")]
        public string MZ_CJJG { get; set; }

        [XmlElement(ElementName = "mz_jsjg")]
        public string MZ_JSJG { get; set; }
        [XmlElement(ElementName = "mz_jzjg")]
        public string MZ_JZJG { get; set; }

        [XmlElement(ElementName = "mz_dysj")]
        public string MZ_DYSJ { get; set; }
        
    }

    public class Tax_QSXX
    {
        [XmlElement(ElementName = "qs_bj")]
        public string QS_BJ { get; set; }

        [XmlElement(ElementName = "qs_fzsj")]
        public string QS_FZSJ { get; set; }

        [XmlElement(ElementName = "qs_xqh")]
        public string QS_XQH { get; set; }

        [XmlElement(ElementName = "qs_nsrsbm")]
        public string QS_NSRSBM { get; set; }

        [XmlElement(ElementName = "qs_mc")]
        public string QS_MC { get; set; }

        [XmlElement(ElementName = "qs_dz")]
        public string QS_DZ { get; set; }

        [XmlElement(ElementName = "qs_mj")]
        public string QS_MJ { get; set; }

        [XmlElement(ElementName = "qs_yt")]
        public string QS_YT { get; set; }

        [XmlElement(ElementName = "qs_zsxm")]
        public string QS_ZSXM { get; set; }

        [XmlElement(ElementName = "qs_zspm")]
        public string QS_ZSPM { get; set; }

        [XmlElement(ElementName = "qs_jsyj")]
        public string QS_JSYJ { get; set; }

        [XmlElement(ElementName = "qs_sl")]
        public string QS_SL { get; set; }

        [XmlElement(ElementName = "qs_jm")]
        public string QS_JM { get; set; }

        [XmlElement(ElementName = "qs_ssqq")]
        public string QS_SSQQ { get; set; }

        [XmlElement(ElementName = "qs_ssqz")]
        public string QS_SSQZ { get; set; }

        [XmlElement(ElementName = "qs_sjje")]
        public string QS_SJJE { get; set; }

        [XmlElement(ElementName = "qs_tpr")]
        public string QS_TPR { get; set; }

        [XmlElement(ElementName = "qs_bz")]
        public string QS_BZ { get; set; }
        
    }
}