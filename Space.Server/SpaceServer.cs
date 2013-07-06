using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Space.Network;

namespace Space.Server
{
    class SpaceServer : ISpaceServer
    {
        public String TestConnection(String test)
        {
            return String.Format("Roger {0} Roger", test);
        }
    }
}
