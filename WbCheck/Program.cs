using System;
using System.Collections.Generic;
using System.Linq; 
using System.Windows.Forms;

namespace WbCheck
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary> 
        static void Main()
        {
            try
            {
                //检查网备数据 First ID 一致
                //Console.WriteLine("开始检查系统中的ID...");
                //Check4ID cid = new Check4ID();
                //string pch = cid.Check();
                //Console.WriteLine("系统中ID不一致，PCH:" + pch);

                //Console.WriteLine("开始检查查封次数...");
                //Check4Seal cSeal = new Check4Seal();
                //pch = cSeal.Check();
                //Console.WriteLine("查封次数不一致，PCH:" + pch);

                //Console.WriteLine("开始检查抵押次数...");
                //Check4DY cdy = new Check4DY();
                //pch = cdy.Check();
                //Console.WriteLine("抵押次数不一致，PCH:" + pch);

                //Console.WriteLine("开始检查只做过查封但是被执行人和网备不一致的");
                //SealPersonCheck spc = new SealPersonCheck();
                //pch = spc.Check();
                //Console.WriteLine("网备中的和被执行人不一致，PCH:" + pch);

                //Console.WriteLine("开始检查首次登记");
                //Check4Initcert ini = new Check4Initcert();
                //  pch = ini.Check();
                //Console.WriteLine("首次未推成功的，PCH:" + pch);

                 Console.WriteLine("开始检查预告登记");
                 Check4YG yg = new Check4YG();
                 string  pch = yg.Check();
                 Console.WriteLine("预告未推成功的，PCH:" + pch);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message); 
            } 
            Console.ReadLine();
        }
    }
}
