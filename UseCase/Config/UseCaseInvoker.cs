using System.Reflection;
using System.Runtime.ExceptionServices;
using UseCase.Core;


namespace UseCase.Config
{
    public class UseCaseInvoker
    {
        private readonly Type usecaseType;
        private readonly IServiceProvider _provider;
        private readonly MethodInfo handleMethod;

        public UseCaseInvoker(Type usecaseType, Type implementsType, IServiceProvider provider)
        {
            this.usecaseType = usecaseType;
            _provider = provider;

            handleMethod = implementsType.GetMethod("Handle");
        }

        public TResponse Invoke<TResponse>(object request)
            where TResponse : IResponse
        {
            var instance = _provider.GetService(usecaseType);

            object? responseObject = null;
            try
            {
                responseObject = handleMethod.Invoke(instance, new[] { request });
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            var response = (TResponse)responseObject;

            return response;
        }
    }
}
