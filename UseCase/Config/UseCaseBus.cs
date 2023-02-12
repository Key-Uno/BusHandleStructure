using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UseCase.Core;

namespace UseCase.Config
{
    public class UseCaseBus
    {
        private readonly Dictionary<Type, Type> handlerTypes = new();
        private readonly ConcurrentDictionary<Type, UseCaseInvoker> invokers = new();

        private IServiceProvider _provider;

        public TResponse Handle<TResponse>(IRequest<TResponse> request)
            where TResponse : IResponse
        {
            var invoker = Invoker(request);
            return invoker.Invoke<TResponse>(request);
        }

        internal void SetUp(IServiceProvider provider)
        {
            _provider = provider;
        }

        internal void Register<TRequest, TUseCase>()
            where TRequest : IRequest<IResponse>
            where TUseCase : IUseCase<TRequest, IResponse>
        {
            handlerTypes.Add(typeof(TRequest), typeof(TUseCase));
        }

        private UseCaseInvoker Invoker<TResponse>(IRequest<TResponse> request)
            where TResponse : IResponse
        {
            var requestType = request.GetType();
            if (invokers.TryGetValue(requestType, out var searchedInvoker))
            {
                return searchedInvoker;
            }

            if (!handlerTypes.TryGetValue(requestType, out var handlerType))
            {
                throw new Exception($"No registered any usecase for this request(RequestType : {request.GetType().Name})");
            }

            var invoker = invokers.GetOrAdd(requestType, _ =>
            {
                var handlerInstance = _provider.GetService(handlerType);
                return new UseCaseInvoker(handlerType, handlerInstance.GetType(), _provider);
            });

            return invoker;
        }
    }
}
