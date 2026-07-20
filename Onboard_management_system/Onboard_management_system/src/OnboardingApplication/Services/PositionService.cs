using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingInfrastructure.Context;

namespace Onboard_management_system.OnboardingApplication.Services;

public class PositionService : IPositionService
{
    private readonly OnboardingDbContext _context;
    private readonly IMapper _mapper;

    public PositionService(OnboardingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PositionDto>> GetAllAsync()
    {
        var positions = await _context.Positions.ToListAsync();
        return _mapper.Map<IEnumerable<PositionDto>>(positions);
    }

    public async Task<PositionDto?> GetByIdAsync(int id)
    {
        var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
        return position is null ? null : _mapper.Map<PositionDto>(position);
    }

    public async Task<PositionDto> CreateAsync(CreatePositionDto dto)
    {
        var position = _mapper.Map<Position>(dto);
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();
        return _mapper.Map<PositionDto>(position);
    }

    public async Task<bool> UpdateAsync(int id, UpdatePositionDto dto)
    {
        var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
        if (position is null) return false;

        _mapper.Map(dto, position);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
        if (position is null) return false;

        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();
        return true;
    }
}