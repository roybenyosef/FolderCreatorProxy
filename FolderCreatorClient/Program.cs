using FolderCreatorInterfaces;
using FolderCreatorProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FolderCreatorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var pipeFactory = 
                new ChannelFactory<IFolderCreator>(new NetNamedPipeBinding(),
                                                   new EndpointAddress("net.pipe://localhost/PipeReverse"));

            IFolderCreator pipeProxy = pipeFactory.CreateChannel();

            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            var result = pipeProxy.CreateFolder("username");
            Console.Write($"Result ok: {result.Success}, error message: {result.ErrorMessage}");
        }
    }
}
