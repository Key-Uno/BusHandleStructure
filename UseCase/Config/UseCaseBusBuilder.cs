using Microsoft.Extensions.DependencyInjection;
using System;
using UseCase.Core;

namespace UseCase.Config
{
    public class UseCaseBusBuilder
    {
        private readonly IServiceCollection _collection;
        private readonly UseCaseBus bus = new();

        public UseCaseBusBuilder(IServiceCollection collection)
        {
            _collection = collection;
        }

        public UseCaseBus Build(IServiceProvider provider)
        {
            bus.SetUp(provider);
            return bus;
        }

        public IServiceCollection RegisterUseCase<TRequest, TImplement>()
            where TRequest : IRequest<IResponse>
            where TImplement : class, IUseCase<TRequest, IResponse>
        {
            _collection.AddSingleton<TImplement>();
            bus.Register<TRequest, TImplement>();

            return _collection;
        }
    }
}
