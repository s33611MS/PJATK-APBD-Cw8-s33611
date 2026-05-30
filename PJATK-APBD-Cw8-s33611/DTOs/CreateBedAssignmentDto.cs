using System.ComponentModel.DataAnnotations;

namespace PJATK_APBD_Cw8_s33611.DTOs;

public record CreateBedAssignmentDto(
    [Required] DateTime From, 
    DateTime? To,
    [Required] string BedType,
    [Required] string Ward
    );