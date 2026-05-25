namespace PJATK_APBD_Cw8_s33611.DTOs;

public record PatientDto(
    string Pesel,
    string FirstName,
    string LastName,
    int Age,
    string Sex,
    IEnumerable<AdmissionDto> Admissions,
    IEnumerable<BedAssignmentDto> BedAssignments
    );