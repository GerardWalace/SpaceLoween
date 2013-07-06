using System;
using System.ServiceModel;

namespace Space.Network
{
    [ServiceContract]
    public interface ISpaceServer
    {
        [OperationContract]
        String TestConnection(String test);
    }
}
