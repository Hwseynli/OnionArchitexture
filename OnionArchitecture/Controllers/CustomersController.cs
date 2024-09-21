using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Features.Commands.Customer.CreateCustomer;
using OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer;
using OnionArchitecture.Application.Features.Queries.Customers;

namespace OnionArchitecture.Controllers;
[Route("api/customer")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICustomerQueries _customerQueries;

    public CustomersController(IMediator mediator, ICustomerQueries customerQueries)
    {
        _mediator = mediator;
        _customerQueries = customerQueries;
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

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetById(int customerId)
    {
        var customer = await _customerQueries.GetByIdAsync(customerId);
        if (customer == null)
            throw new NotFoundException();

        return Ok(customer);
    }

    [HttpGet("{customerId}/documents")]
    public async Task<IActionResult> GetCustomerDocuments(int customerId)
    {
        return await _customerQueries.GetCustomerDocuments(customerId);
    }

    [HttpGet("{customerId}/documents/{additionDocumentId}")]
    public async Task<IActionResult> DownloadDocuments(int additionDocumentId)
    {
        return await _customerQueries.DownloadDocuments(additionDocumentId);
    }

    [HttpGet("{customerId}/documents")]
    public async Task<IActionResult> GetDocumentsWithTypes(int customerId)
    {
        return await _customerQueries.GetDocumentsWithTypes(customerId);
    }
}
