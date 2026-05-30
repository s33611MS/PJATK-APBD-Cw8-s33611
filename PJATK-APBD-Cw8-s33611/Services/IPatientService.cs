using PJATK_APBD_Cw8_s33611.DTOs;

namespace PJATK_APBD_Cw8_s33611.Services;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetPatientsAsync(string? search, CancellationToken cancellationToken);
    Task AddBedAssignmentAsync(string pesel, CreateBedAssignmentDto request, CancellationToken cancellationToken);
}