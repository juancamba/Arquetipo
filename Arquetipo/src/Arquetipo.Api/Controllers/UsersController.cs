using Arquetipo.Api.Dto.Users;
using Arquetipo.Application.Users.CreateUser;
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
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserDto user, CancellationToken cancellationToken)
        {
            var commnad = new CreateUserCommand(user.Id, user.Name);
            var result = await _mediator.Send(commnad, cancellationToken);

            return Ok(result);
        }
    }
}
