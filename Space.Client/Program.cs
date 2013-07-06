using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Space.Network;

namespace Space.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Space.Client");

            using (ChannelFactory<ISpaceServer> spaceFactory = new ChannelFactory<ISpaceServer>("Space.Client"))
            {
                ISpaceServer serverProxy = spaceFactory.CreateChannel();
                string test = serverProxy.TestConnection("Client");
                Console.WriteLine(test);
            }
        }
    }
}
