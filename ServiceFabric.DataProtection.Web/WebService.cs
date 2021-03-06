﻿using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

using System.Collections.Generic;
using System.Fabric;

namespace ServiceFabric.DataProtection.Web
{
    internal sealed class WebService : StatelessService
    {
        ServerType _serverType;

        public WebService(StatelessServiceContext context, ServerType serverType)
            : base(context)
        {
            _serverType = serverType;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                {
                    switch (_serverType)
                    {
                        case ServerType.WebListener :
                            {
                                return new WebListenerCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                                {
                                    return WebHostBuilderHelper.GetServiceFabricWebHost(_serverType);
                                });
                            }
                        case ServerType.Kestrel:
                            {
                                return new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                                {
                                    return WebHostBuilderHelper.GetServiceFabricWebHost(_serverType);
                                });
                            }
                        default:
                            return null;
                    }
                })                  
            };
        }
    }
}