using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Exceptions;
using OnionArchitecture.Application.Features.Commands.Customer.CreateCustomer;
using OnionArchitecture.Application.Features.Commands.Customer.UpdateCustomer;
using OnionArchitecture.Application.Features.Queries.Customers;
using OnionArchitecture.Application.Interfaces.IManagers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnionArchitecture.Controllers;
[Route("api/customer")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDocumentManager _documentManager;
    private readonly ICustomerQueries _customerQueries;


    public CustomersController(IMediator mediator, IDocumentManager documentManager, ICustomerQueries customerQueries)
    {
        _mediator = mediator;
        _documentManager = documentManager;
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

    [HttpGet("{customerId}/documents/{additionDocumentId}")]
    public async Task<IActionResult> DownloadDocuments(int customerId, int additionDocumentId)
    {
        return await _documentManager.DownloadDocuments(customerId, additionDocumentId);
    }

}

