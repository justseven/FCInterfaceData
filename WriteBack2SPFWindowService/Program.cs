using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace WriteBack2SPFWindowService
{
    class Program
    {
        static void Main(string[] args)
        {
            //var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            //XmlConfigurator.ConfigureAndWatch(logCfg);
            XmlConfigurator.Configure();
            HostFactory.Run(x =>
            {
                x.UseLog4Net();
                x.Service<ServiceRunner>();
                x.SetDescription("轮回房产数据写回服务。");
                x.SetDisplayName("数据写回");
                x.SetServiceName("轮回房产数据写回服务");

                x.EnablePauseAndContinue();
            });
            //HostFactory.Run(x =>                                 
            //{                                                    
            //    x.Service<TownCrier>();                          
            //    x.RunAsLocalSystem();                            
            //    x.SetDescription("Sample Topshelf Host");        
            //    x.SetDisplayName("Stuff");                       
            //    x.SetServiceName("Stuff");                       
            //});
            //Console.WriteLine("Begin Run");
            //XmlConfigurator.Configure();
            //Type type = MethodBase.GetCurrentMethod().DeclaringType;
            //ILog m_log = LogManager.GetLogger(type);
            //m_log.Debug("这是一个Debug日志");
            //m_log.Info("这是一个Info日志");
            //m_log.Warn("这是一个Warn日志");
            //m_log.Error("这是一个Error日志");
            //m_log.Fatal("这是一个Fatal日志");
            //Console.WriteLine("End");
            //Console.ReadLine();
            //Console.Read();
        }
    }
}
