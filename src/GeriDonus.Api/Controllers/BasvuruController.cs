using GeriDonus.Service;
using Microsoft.AspNetCore.Mvc;

namespace GeriDonus.Api.Controllers;

[ApiController]
[Route("api/basvurular")]
public class BasvuruController : ControllerBase
{
    private readonly BasvuruService _service;

    public BasvuruController(BasvuruService service)
    {
        _service = service;
    }

    // GET /api/basvurular?memurIlId=6&ilId=6&sayfa=1&boyut=20
    [HttpGet]
    public async Task<IActionResult> Listele(int memurIlId, int ilId, int sayfa = 1, int boyut = 20)
    {
        var sonuc = await _service.IlinBasvurulariAsync(memurIlId, ilId, sayfa, boyut);

        return sonuc.Durum switch
        {
            SonucDurumu.Basarili      => Ok(sonuc.Veri),
            SonucDurumu.Yetkisiz      => StatusCode(403, new { mesaj = sonuc.Mesaj }),
            SonucDurumu.GecersizIstek => StatusCode(400, new { mesaj = sonuc.Mesaj }),
            _                         => StatusCode(500)
        };
    }
}