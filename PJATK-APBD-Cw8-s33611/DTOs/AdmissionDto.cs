namespace PJATK_APBD_Cw8_s33611.DTOs;

public record AdmissionDto(
    int Id,
    DateTime AdmissionDate,
    DateTime? DischargeDate,
    WardDto Ward
    );