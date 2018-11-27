using FolderCreatorInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FolderCreatorProxy
{
    public partial class FolderCreatorProxyService : ServiceBase
    {
        ServiceHost serviceHost = null;

        public FolderCreatorProxyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                serviceHost = new ServiceHost(typeof(FolderCreator), new Uri[]{ new Uri("net.pipe://localhost")});
                serviceHost.AddServiceEndpoint(typeof(IFolderCreator), new NetNamedPipeBinding(), "PipeReverse");
                serviceHost.Open();
            }
            catch (Exception ex)
            {
                EventViewerLogger.WriteErrorEvent(ex.Message);
            }
            EventViewerLogger.WriteInfoEvent("Service Initialization Completed");
        }

        protected override void OnStop()
        {
            serviceHost.Close();
        }
    }
}
