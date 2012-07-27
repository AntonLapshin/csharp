using System;
using System.ServiceModel;

namespace ChatWcfService
{
    public abstract class ServiceHostFactory
    {
        public abstract ServiceHost CreateServiceHost(Type serviceType,
                                                      Uri[] baseAddresses);
    }
}