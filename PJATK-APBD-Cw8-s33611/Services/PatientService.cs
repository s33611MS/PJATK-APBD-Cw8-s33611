using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PJATK_APBD_Cw8_s33611.DTOs;
using PJATK_APBD_Cw8_s33611.Infrastructure;

namespace PJATK_APBD_Cw8_s33611.Services;

public class PatientService (_2019sbdContext ctx) : IPatientService
{
    public async Task<IEnumerable<PatientDto>> GetPatientsAsync(string? search, CancellationToken cancellationToken)
    {
        return await ctx.Patients
            .Where(p => search.IsNullOrEmpty() || p.FirstName.Contains(search) || p.LastName.Contains(search))
            .Select(p => new PatientDto(
            p.Pesel,
            p.FirstName,
            p.LastName,
            p.Age,
            p.Sex ? "Male" : "Female",
            p.Admissions.Select(a => new AdmissionDto(
                a.Id,
                a.AdmissionDate,
                a.DischargeDate,
                new WardDto(
                    a.Ward.Id,
                    a.Ward.Name,
                    a.Ward.Description
                    )
                )),
            p.BedAssignments.Select(ba => new BedAssignmentDto(
                ba.Id,
                ba.From,
                ba.To,
                new BedDto(
                    ba.Bed.Id,
                    new BedTypeDto(
                        ba.Bed.BedType.Id,
                        ba.Bed.BedType.Name,
                        ba.Bed.BedType.Description
                        ),
                    new RoomDto(
                        ba.Bed.Room.Id,
                        ba.Bed.Room.HasTv,
                        new WardDto(
                            ba.Bed.Room.Ward.Id,
                            ba.Bed.Room.Ward.Name,
                            ba.Bed.Room.Ward.Description)
                        )
                    )
                )
            )
        )).ToListAsync(cancellationToken);
    }
}