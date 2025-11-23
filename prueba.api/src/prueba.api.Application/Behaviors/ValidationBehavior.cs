using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace prueba.api.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
    {
        private readonly IServiceProvider _provider;

        public ValidationBehavior(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
        {

            Console.WriteLine("ValidationBehavior invoked.");
            using var scope = _provider.CreateScope();
            var validators = scope.ServiceProvider.GetServices<IValidator<TRequest>>();

            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(message);
                var failures = validators
                    .Select(v => v.Validate(context))
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }
            }


            return await next(message, cancellationToken);
        }
    }
}