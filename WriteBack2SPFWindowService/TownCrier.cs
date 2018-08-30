using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace WriteBack2SPFWindowService
{
    public class TownCrier : ServiceControl
    {
        private Timer _timer = null;
        readonly ILog _log = LogManager.GetLogger("TownCrierAppender");
        public TownCrier()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => _log.Error(DateTime.Now);

        }
        public bool Start(HostControl hostControl)
        {
            _log.Error("TopshelfDemo is Started");
            _timer.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            throw new NotImplementedException();
        }
        public bool Continue(HostControl hostControl)
        {
            _log.Info("TopshelfDemo is Continued");
            _timer.Start();
            return true;
        }
        public bool Pause(HostControl hostControl)
        {
            _log.Info("TopshelfDemo is stoped");
            _timer.Stop();
            return true;
        }
    }
}
