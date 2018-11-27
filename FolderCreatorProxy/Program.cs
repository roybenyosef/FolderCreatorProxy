using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FolderCreatorProxy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Install service: sc create FolderCreatorProxy binpath=.\FolderCreatorProxy.exe start=auto DisplayName="Folder Creator Proxy"
        /// Delete service: sc delete FolderCreatorProxy
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FolderCreatorProxyService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
