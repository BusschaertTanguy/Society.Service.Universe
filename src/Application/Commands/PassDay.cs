using System;
using System.Threading.Tasks;
using Application.Transactions;
using Domain.Entities;
using Domain.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public static class PassDay
    {
        public class Command : IRequest
        {
        }

        internal class Handler : CommandHandler<Command>
        {
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly IUniverseRepository _universeRepository;

            public Handler(ILogger<CommandHandler<Command>> logger, IUnitOfWork unitOfWork, IUniverseRepository universeRepository, IPublishEndpoint publishEndpoint) : base(logger, unitOfWork)
            {
                _universeRepository = universeRepository;
                _publishEndpoint = publishEndpoint;
            }

            protected override async Task Process(Command command)
            {
                var universe = await _universeRepository.GetActive();

                if (universe == null)
                {
                    universe = new Universe(Guid.NewGuid());
                    await _universeRepository.Add(universe);
                }
                else
                {
                    universe.PassDay();
                }

                foreach (var domainEvent in universe.DomainEvents)
                {
                    await _publishEndpoint.Publish(domainEvent);
                }

                universe.ClearEvents();
            }
        }
    }
}