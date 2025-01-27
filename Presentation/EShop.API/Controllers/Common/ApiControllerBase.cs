using EShop.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers.Common
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        protected Guid AuthenticatedUserId => Guid.Parse(User?.Claims.GetId() ?? throw new ArgumentNullException(nameof(User)));
    }
}
