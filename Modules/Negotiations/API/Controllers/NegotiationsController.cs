﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Negotiations.Application.Commands.AcceptNegotiation;
using Negotiations.Application.Commands.ProposePrice;
using Negotiations.Application.Commands.RejectNegotiation;
using Negotiations.Application.Commands.StartNegotiation;
using Negotiations.Application.DTOs;
using Negotiations.Application.Queries.GetNegotiation;
using Shared.API.Common;

namespace Negotiations.API.Controllers;

[ApiController]
[Route("api/negotiations")]
public class NegotiationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<NegotiationsController> _logger;

    public NegotiationsController(IMediator mediator, ILogger<NegotiationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NegotiationDto>>> Create(
        [FromBody] StartNegotiationRequestDto dto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to start negotiation");
        var command = new StartNegotiationCommand(dto.CustomerId, dto.ProductId);
        var result = await _mediator.Send(command, cancellationToken);
        
        _logger.LogInformation("Negotiation started successfully with ID: {Id}", result.Id);
        return Ok(ApiResponse<NegotiationDto>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<NegotiationDto>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request for negotiation {Id}", id);
        var result = await _mediator.Send(new GetNegotiationQuery(id), cancellationToken);
        if (result == null)
        {
            _logger.LogWarning("Negotiation not found with ID {Id}", id);
            return NotFound(ApiResponse<NegotiationDto>.Fail("Negotiation not found"));
        }
        
        _logger.LogInformation("Successfully retrieved negotiation {Id}", id);
        return Ok(ApiResponse<NegotiationDto>.Ok(result));
    }

    [HttpPost("{id:guid}/propose")]
    public async Task<ActionResult<ApiResponse<string>>> Propose(
        Guid id,
        [FromBody] ProposePriceRequestDto dto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received propose for negotiation {Id}", id);
        var command = new ProposePriceCommand(id, dto.NewPrice);
        await _mediator.Send(command, cancellationToken);
        
        _logger.LogInformation("Price proposal submitted successfully for negotiation {Id}", id);
        return Ok(ApiResponse<string>.Ok("Proposal submitted"));
    }

    [HttpPost("{id:guid}/accept")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<string>>> Accept(
        Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received accept for negotiation {Id}", id);
        var command = new AcceptNegotiationCommand(id);
        await _mediator.Send(command, cancellationToken);
        
        _logger.LogInformation("Successfully accepted negotiation {Id}", id);
        return Ok(ApiResponse<string>.Ok("Negotiation accepted"));
    }

    [HttpPost("{id:guid}/reject")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<string>>> Reject(
        Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received reject for negotiation {Id}", id);
        var command = new RejectNegotiationCommand(id);
        await _mediator.Send(command, cancellationToken);
        
        _logger.LogInformation("Successfully rejected negotiation {Id}", id);
        return Ok(ApiResponse<string>.Ok("Negotiation rejected"));
    }
}