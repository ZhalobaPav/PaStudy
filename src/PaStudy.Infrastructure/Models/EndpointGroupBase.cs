using Microsoft.AspNetCore.Builder;

namespace PaStudy.Infrastructure.Models;

public abstract class EndpointGroupBase
{
    public abstract void Map(WebApplication app);
}
