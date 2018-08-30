using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Web4BDC.Dal; 

namespace Web4BDC.Bll
{
    public class BDCInfo4WWW
    {
        /// <summary>
        /// 获取不动产的办件步骤信息
        /// </summary>
        /// <returns></returns>
        public void GetBdcStepInfo(string fileName)
        {
            BDCInfo4WWWDal dal = new BDCInfo4WWWDal();
            DataTable dt = dal.GetBDCStepInfo();
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("不动产办理步骤信息");
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("业务编号");
            row.CreateCell(1).SetCellValue("查询密码");
            row.CreateCell(2).SetCellValue("通知人");
            row.CreateCell(3).SetCellValue("流程步骤");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    NPOI.SS.UserModel.IRow row1 = sheet.CreateRow(i + 1);
                    row1.CreateCell(0).SetCellValue(dt.Rows[i]["SLBH"].ToString());
                    row1.CreateCell(1).SetCellValue(dt.Rows[i]["CXMM"].ToString());
                    row1.CreateCell(2).SetCellValue(dt.Rows[i]["TZRXM"].ToString());
                    row1.CreateCell(3).SetCellValue(dt.Rows[i]["StepName"].ToString());
                }
            }

            using (FileStream stm = File.OpenWrite(fileName))
            {
                book.Write(stm);
            }
        }
        public void GetBdcStepInfoT(string fileName)
        {
            BDCInfo4WWWDal dal = new BDCInfo4WWWDal();
            DataTable dt = dal.GetBDCStepInfoT();
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("不动产办理步骤信息");
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("受理编号");
            row.CreateCell(1).SetCellValue("不动产权证号");
            row.CreateCell(2).SetCellValue("通知人");
            row.CreateCell(3).SetCellValue("坐落");
            row.CreateCell(4).SetCellValue("申请类型");
            row.CreateCell(5).SetCellValue("接件日期");
            row.CreateCell(6).SetCellValue("项目状态");
            row.CreateCell(7).SetCellValue("行政区代码");
            row.CreateCell(8).SetCellValue("区域");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    NPOI.SS.UserModel.IRow row1 = sheet.CreateRow(i + 1);
                    row1.CreateCell(0).SetCellValue(dt.Rows[i]["受理编号"].ToString());
                    row1.CreateCell(1).SetCellValue(dt.Rows[i]["不动产权证号"].ToString());
                    row1.CreateCell(2).SetCellValue(dt.Rows[i]["通知人"].ToString());
                    row1.CreateCell(3).SetCellValue(dt.Rows[i]["坐落"].ToString());
                    row1.CreateCell(4).SetCellValue(dt.Rows[i]["申请类型"].ToString());
                    row1.CreateCell(5).SetCellValue(dt.Rows[i]["接件日期"].ToString());
                    row1.CreateCell(6).SetCellValue(dt.Rows[i]["项目状态"].ToString());
                    row1.CreateCell(7).SetCellValue(dt.Rows[i]["行政区代码"].ToString());
                    row1.CreateCell(8).SetCellValue(dt.Rows[i]["区域"].ToString());

                }
            }

            using (FileStream stm = File.OpenWrite(fileName))
            {
                book.Write(stm);
            }
        }
    }
}