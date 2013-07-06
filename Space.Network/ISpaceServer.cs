using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Space.Network
{
    [ServiceContract]
    public interface ISpaceServer
    {
        [OperationContract]
        String TestConnection(String test);
    }
}
