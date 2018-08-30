using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web4BDC.Tools
{
    public class EMFChange
    {
        /// <summary>
        /// 转为JPG
        /// <param name="sourcePath">目标目录</param>
        /// <returns>已转换的文件数量</returns>
        public int ConvertToJGP(string sourcePath)
        {
            string[] files = FileHelper.GetFileNames(sourcePath, "*.emf", true);
            int count = 0;
            foreach (string filePath in files)
            {
                try
                {
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(new System.Drawing.Imaging.Metafile(filePath));
                    string jpgPath = filePath.Replace(".emf", ".jpg");
                    bmp.Save(jpgPath);
                    count++;
                }
                catch
                {
                    continue;
                }
            }
            return count; ;
        }
    }
}