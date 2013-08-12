using System;
using System.ServiceModel;

namespace Space.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Pi !");

            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
