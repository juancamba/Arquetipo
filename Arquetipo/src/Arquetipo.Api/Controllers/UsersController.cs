using Arquetipo.Api.Dto.Users;
using Arquetipo.Application.Users.CreateUser;
using Arquetipo.Application.Users.GetAll;
using Arquetipo.Hexagonal.Application.Extensions;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Arquetipo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;


        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IResult> Create([FromBody] UserDto user, CancellationToken cancellationToken)
        {
            var commnad = new CreateUserCommand(user.Id, user.Name);
            var result = await _mediator.Send(commnad, cancellationToken);

            if (result.IsError)
            {
                return result.Errors.ToProblem();
            }
            return Results.Ok(result.Value);
        }

        [HttpGet()]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);

            if (result.IsError)
            {
                return result.Errors.ToProblem();
            }
            return Results.Ok(result.Value);
        }
    }
}
