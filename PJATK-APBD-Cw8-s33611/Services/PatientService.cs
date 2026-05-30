using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PJATK_APBD_Cw8_s33611.DTOs;
using PJATK_APBD_Cw8_s33611.Exceptions;
using PJATK_APBD_Cw8_s33611.Infrastructure;
using PJATK_APBD_Cw8_s33611.Models;

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

    public async Task AddBedAssignmentAsync(string pesel, CreateBedAssignmentDto request, CancellationToken cancellationToken)
    {
        if (!await ctx.Patients.AnyAsync(p => p.Pesel == pesel, cancellationToken))
            throw new NotFoundException($"There is no patient with pesel: {pesel}");
        
        if (!await ctx.BedTypes.AnyAsync(bt => bt.Name.Trim().ToLower() == request.BedType.Trim().ToLower(), cancellationToken))
            throw new NotFoundException($"There is no {request.BedType} bed type");
        
        if (!await ctx.Wards.AnyAsync(w => w.Name.Trim().ToLower() == request.Ward.Trim().ToLower(), cancellationToken))
            throw new NotFoundException($"There is no {request.Ward} ward");
        
        if (!await ctx.Beds.AnyAsync(b => b.BedType.Name.Trim().ToLower() == request.BedType.Trim().ToLower() && b.Room.Ward.Name.Trim().ToLower() == request.Ward.Trim().ToLower(), cancellationToken))
            throw new NotFoundException($"There is no room of {request.BedType} bed type in ward {request.Ward}");
        
        var bed = await ctx.Beds
            .Where(b => 
                b.BedType.Name.ToLower() == request.BedType.ToLower() && 
                b.Room.Ward.Name.ToLower() == request.Ward.ToLower() && 
                !b.BedAssignments.Any(ba => 
                    (ba.To == null && (ba.From <= request.From || ba.From <= request.To)) ||
                    (ba.From >= request.From && ba.To <= request.To) ||
                    (ba.From <= request.To && ba.To >= request.To) ||
                    (ba.From <= request.From && ba.To >= request.From) ||
                    (request.To == null && ba.From > request.From)
                ))
            .Select(b => b.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        var to = request.To ?? DateTime.Now;
        if (bed == 0)
            throw new NotFoundException($"There is no free bed with type {request.BedType} in ward {request.Ward} between {request.From} and {to}");
        
        var assignment = new BedAssignment()
        {
            PatientPesel = pesel,
            BedId = bed,
            From = request.From,
            To = request.To
        };

        ctx.Add(assignment);
        await ctx.SaveChangesAsync(cancellationToken);
    }
}