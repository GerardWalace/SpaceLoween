using System;
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
