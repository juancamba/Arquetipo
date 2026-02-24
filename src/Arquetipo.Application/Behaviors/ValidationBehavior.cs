using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Application.Exceptions;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Arquetipo.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
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
           
                var validationErrors = validators
                    .Select(validators => validators.Validate(context))
                    .Where(validationResult => validationResult.Errors.Any())
                    .SelectMany(validationResult => validationResult.Errors)
                    .Select(validationFailure => new ValidationError(
                        validationFailure.PropertyName,
                        validationFailure.ErrorMessage
                    )).ToList();

                if (validationErrors.Any())
                {
                    throw new Exceptions.ValidationException(validationErrors);
                }

            }


            return await next(message, cancellationToken);
        }
    }
}