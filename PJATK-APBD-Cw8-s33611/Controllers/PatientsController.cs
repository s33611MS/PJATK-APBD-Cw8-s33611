using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw8_s33611.DTOs;
using PJATK_APBD_Cw8_s33611.Exceptions;
using PJATK_APBD_Cw8_s33611.Services;

namespace PJATK_APBD_Cw8_s33611.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] string? search, CancellationToken cancellationToken)
    {
        return Ok(await service.GetPatientsAsync(search, cancellationToken));
    }
    
    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AddBedAssignmentAsync([FromRoute] string pesel, [FromBody] CreateBedAssignmentDto request, CancellationToken cancellationToken)
    {
        try
        {
            await service.AddBedAssignmentAsync(pesel, request, cancellationToken);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}