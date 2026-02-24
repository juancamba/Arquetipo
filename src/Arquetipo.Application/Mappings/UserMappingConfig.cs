

using Arquetipo.Application.Shared.Users;
using Arquetipo.Domain.Users;
using Mapster;

namespace Arquetipo.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Domain → Application DTO
        config.NewConfig<User, UserResponse>();
    }
}
