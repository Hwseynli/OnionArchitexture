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

    // Pagination və tarix filtrasiyası ilə birgə müştərilərin siyahısını əldə etmək üçün GET metodu
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,        // Default olaraq səhifə 1
        [FromQuery] int pageSize = 10,         // Default olaraq səhifədə 10 element
        [FromQuery] DateTime? fromDate = null, // Tarix filtrasiyası üçün optional olaraq fromDate
        [FromQuery] DateTime? toDate = null    // Tarix filtrasiyası üçün optional olaraq toDate
    )
    {
        // Gələn sorğunu yoxlayırıq və səhifə və ölçü üçün minimal dəyərlər veririk
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;

        // Customer məlumatlarını əldə edirik
        var result = await _customerQueries.GetAllAsync(pageNumber, pageSize, fromDate, toDate);

        // Pagination nəticəsi boş olduqda 404 qaytarırıq
        if (result.Items.Count == 0)
        {
            return NotFound("No customers found for the given filters.");
        }

        // Pagination nəticəsi varsa 200 qaytarırıq
        return Ok(result);
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

    [HttpGet("{customerId}/documentTypes")]
    public async Task<IActionResult> GetDocumentsWithTypes(int customerId)
    {
        return await _customerQueries.GetDocumentsWithTypes(customerId);
    }
}
