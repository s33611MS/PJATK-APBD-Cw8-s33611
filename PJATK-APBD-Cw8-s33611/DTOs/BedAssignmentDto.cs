namespace PJATK_APBD_Cw8_s33611.DTOs;

public record BedAssignmentDto(
    int Id,
    DateTime From,
    DateTime? To,
    BedDto Bed
    );