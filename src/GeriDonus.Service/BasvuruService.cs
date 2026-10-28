using GeriDonus.Data.Entities;
using GeriDonus.Data.Repositories;

namespace GeriDonus.Service;

public enum SonucDurumu { Basarili, Yetkisiz, GecersizIstek }

public record Sonuc<T>(SonucDurumu Durum, T? Veri = default, string? Mesaj = null);

public class BasvuruService
{
    private readonly BasvuruRepository _repo;

    public BasvuruService(BasvuruRepository repo)
    {
        _repo = repo;
    }

    public async Task<Sonuc<List<Basvuru>>> IlinBasvurulariAsync(
        int memurIlId, int ilId, int sayfa, int boyut)
    {
        if (memurIlId != ilId)
        {
            return new Sonuc<List<Basvuru>>(SonucDurumu.Yetkisiz, Mesaj: "Yetkisiz erişim. Başvurular sadece kendi iliniz için görüntülenebilir.");
        }
        if (sayfa < 1 || boyut < 1 || boyut > 100)
        {
            return new Sonuc<List<Basvuru>>(SonucDurumu.GecersizIstek, Mesaj: "Geçersiz istek. Sayfa en az 1, boyut 1 ile 100 arasında olmalıdır.");
        }
        var liste = await _repo.IlinBasvurulariAsync(ilId, sayfa, boyut);
        return new Sonuc<List<Basvuru>>(SonucDurumu.Basarili, liste);
    }
}