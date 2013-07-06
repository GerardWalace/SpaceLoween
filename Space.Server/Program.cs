using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Space.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(SpaceServer)))
            {
                host.Open();
                Console.WriteLine("Server Started");
                Console.WriteLine("Press 'Enter' to stop");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
