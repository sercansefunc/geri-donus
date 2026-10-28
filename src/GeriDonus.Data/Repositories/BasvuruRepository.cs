using Microsoft.EntityFrameworkCore;
using GeriDonus.Data.Entities;

namespace GeriDonus.Data.Repositories;

public class BasvuruRepository
{
    private readonly GeriDonusContext _context;

    public BasvuruRepository(GeriDonusContext context)
    {
        _context = context;
    }

    public async Task<List<Basvuru>>  IlinBasvurulariAsync(int ilId, int sayfa, int boyut)
    {
        return await _context.Basvurus
            .Where(b => b.MudurlukIlId == ilId)
            .OrderBy(b => b.Basvurutarihi)
            .ThenBy(b => b.Id)
            .Skip((sayfa - 1) * boyut)
            .Take(boyut)
            .ToListAsync();
    }
}