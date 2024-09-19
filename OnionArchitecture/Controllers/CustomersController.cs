using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Commands.Customer.CreateCustomer;
using OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnionArchitecture.Controllers;
[Route("api/customer")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCustomerCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : BadRequest();
    }
}



