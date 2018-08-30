using FCWriteBack;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteBack2SPFWindowService
{
    public sealed class Write2ServiceJob:IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Write2ServiceJob));
        public void Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("-------------开始------------");
            Polling p = new Polling();
            p.GetOnce();
            _logger.InfoFormat("-------------结束------------");
            _logger.InfoFormat("-------------收费开始------------");
            p.RePushSF();
            _logger.InfoFormat("-------------收费结束------------");
        }
    }
}
