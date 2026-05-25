using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw8_s33611.DTOs;
using PJATK_APBD_Cw8_s33611.Services;

namespace PJATK_APBD_Cw8_s33611.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, CancellationToken cancellationToken)
    {
        return Ok(await service.GetPatientsAsync(search, cancellationToken));
    }
}