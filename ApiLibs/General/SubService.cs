﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibs.General
{
    public abstract class SubService<T> : Service where T : Service
    {
        public T Service { get; }

        protected SubService(T service) : base(service.Implementation)
        {
            Service = service;
        }

        protected SubService(T service, string endpoint) : this(service)
        {
            if (!string.IsNullOrEmpty(endpoint))
            {
                RequestMiddleware.Add((req) =>
                {
                    req.EndPoint = endpoint + "/" + req.EndPoint;
                    return Task.FromResult(req);
                });
            }
        }


        protected internal override Task<RequestResponse> HandleRequest(Request request, IEnumerable<Func<Request, Task<Request>>> requestMiddleware = null, IEnumerable<Func<RequestResponse, Task<RequestResponse>>> requestResponseMiddleware = null)
        {
            requestMiddleware ??= new List<Func<Request, Task<Request>>>();
            requestResponseMiddleware ??= new List<Func<RequestResponse, Task<RequestResponse>>>();
            return base.HandleRequest(request, RequestMiddleware.Concat(requestMiddleware), RequestResponseMiddleware.Concat(requestResponseMiddleware));
        }
    }
}
