using PaStudy.Core.Entities;
using PaStudy.Core.Interfaces;
using PaStudy.Infrastructure.ConfigureDependencies;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;

namespace PavStudy.API.Endpoints
{
    public class Students : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                ;
        }
    }
}
