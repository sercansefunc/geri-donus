-- 02_veri.sql
-- GeriDonus veritabanina test verisi uretir.
-- Onkosul: 01_sema.sql calistirilmis ve tablolar bos olmali.
-- Hedef: 81 il, 6 durum, 300.000 aile, 1.000.000 kisi, 250 personel,
--        300.000 basvuru, ~1.050.000 durum gecmisi satiri.
-- DBeaver notu: degisken iceren bloklari secip Ctrl+Enter ile tek parca calistir.

USE GeriDonus;
GO
SET NOCOUNT ON;

------------------------------------------------------------
-- 1. Iller (plaka sirasiyla 81 il)
------------------------------------------------------------
INSERT INTO Il (ad) VALUES
(N'Adana'),(N'Adıyaman'),(N'Afyonkarahisar'),(N'Ağrı'),(N'Amasya'),(N'Ankara'),(N'Antalya'),(N'Artvin'),(N'Aydın'),(N'Balıkesir'),
(N'Bilecik'),(N'Bingöl'),(N'Bitlis'),(N'Bolu'),(N'Burdur'),(N'Bursa'),(N'Çanakkale'),(N'Çankırı'),(N'Çorum'),(N'Denizli'),
(N'Diyarbakır'),(N'Edirne'),(N'Elazığ'),(N'Erzincan'),(N'Erzurum'),(N'Eskişehir'),(N'Gaziantep'),(N'Giresun'),(N'Gümüşhane'),(N'Hakkari'),
(N'Hatay'),(N'Isparta'),(N'Mersin'),(N'İstanbul'),(N'İzmir'),(N'Kars'),(N'Kastamonu'),(N'Kayseri'),(N'Kırklareli'),(N'Kırşehir'),
(N'Kocaeli'),(N'Konya'),(N'Kütahya'),(N'Malatya'),(N'Manisa'),(N'Kahramanmaraş'),(N'Mardin'),(N'Muğla'),(N'Muş'),(N'Nevşehir'),
(N'Niğde'),(N'Ordu'),(N'Rize'),(N'Sakarya'),(N'Samsun'),(N'Siirt'),(N'Sinop'),(N'Sivas'),(N'Tekirdağ'),(N'Tokat'),
(N'Trabzon'),(N'Tunceli'),(N'Şanlıurfa'),(N'Uşak'),(N'Van'),(N'Yozgat'),(N'Zonguldak'),(N'Aksaray'),(N'Bayburt'),(N'Karaman'),
(N'Kırıkkale'),(N'Batman'),(N'Şırnak'),(N'Bartın'),(N'Ardahan'),(N'Iğdır'),(N'Yalova'),(N'Karabük'),(N'Kilis'),(N'Osmaniye'),(N'Düzce');

------------------------------------------------------------
-- 2. Durumlar (id sirasi = surec sirasi)
------------------------------------------------------------
INSERT INTO Durum (ad) VALUES
(N'Kayıt alındı'),
(N'Kontrol edildi'),
(N'Form dolduruldu'),
(N'Yol izin belgesi verildi'),
(N'Sınırdan çıkış'),
(N'Kayıt sonlandırıldı');

------------------------------------------------------------
-- 3. Aileler: 300.000 (tek kolon IDENTITY oldugu icin IDENTITY_INSERT)
--    Uc satir birlikte calistirilmali.
------------------------------------------------------------
SET IDENTITY_INSERT Aile ON;
INSERT INTO Aile (id) SELECT value FROM GENERATE_SERIES(1, 300000);
SET IDENTITY_INSERT Aile OFF;
-- Not: Sonra DEFAULT VALUES ile eklenen aile 300.001 alir;
-- IDENTITY sayaci en buyuk elle yazilan degere tasinir.

------------------------------------------------------------
-- 4. Kisiler: 1.000.000, set-based (olculen sure: ~5 sn)
------------------------------------------------------------
INSERT INTO Kisi (ad, soyad, dogumtarihi, aile_id, il_id)
SELECT
    N'Ad_' + CAST(value AS NVARCHAR(10)),
    N'Soyad_' + CAST(value AS NVARCHAR(10)),
    DATEADD(DAY, -(value % 25000), '2020-01-01'),   -- 1952-2020 arasi
    (value % 300000) + 1,
    (value % 81) + 1
FROM GENERATE_SERIES(1, 1000000);

------------------------------------------------------------
-- 5. Personel: 250, her ilin ilk personeli mudur, ikincisi uzman
------------------------------------------------------------
INSERT INTO Personel (il_id, ad, soyad, rol)
SELECT
    value % 81 + 1,
    N'Ad_' + CAST(value AS NVARCHAR(10)),
    N'Soyad_' + CAST(value AS NVARCHAR(10)),
    CASE
        WHEN value <= 81              THEN N'Müdür'
        WHEN value BETWEEN 82 AND 162 THEN N'Uzman'
        ELSE N'Memur'
    END
FROM GENERATE_SERIES(1, 250);

------------------------------------------------------------
-- 6. Basvurular: ilk 300.000 kisi, mudurluk = kisinin kendi ili
--    (olculen sure: ~1,4 sn)
------------------------------------------------------------
INSERT INTO Basvuru (kisi_id, mudurluk_il_id, basvurutarihi)
SELECT
    id,
    il_id,
    DATEADD(DAY, -(id % 365), CAST(SYSDATETIME() AS DATE))   -- son 1 yil
FROM Kisi
WHERE id <= 300000;

------------------------------------------------------------
-- 7. Durum gecmisi: her basvuruya 1-6 durum (olculen sure: ~6,6 sn)
--    CROSS JOIN ile her basvuru 6'ya katlanir, WHERE fazlasini atar.
------------------------------------------------------------
INSERT INTO BasvuruDurumGecmisi (basvuru_id, durum_id, personel_id, islemzamani)
SELECT
    b.id,
    g.value,                                  -- n. durum = durum_id
    (b.id % 250) + 1,
    DATEADD(DAY, g.value, b.basvurutarihi)    -- her durum bir gun sonra
FROM Basvuru b
CROSS JOIN GENERATE_SERIES(1, 6) g
WHERE g.value <= b.id % 6 + 1;

------------------------------------------------------------
-- Kontrol
------------------------------------------------------------
SELECT 'Il' AS tablo, COUNT(*) AS satir FROM Il
UNION ALL SELECT 'Durum', COUNT(*) FROM Durum
UNION ALL SELECT 'Aile', COUNT(*) FROM Aile
UNION ALL SELECT 'Kisi', COUNT(*) FROM Kisi
UNION ALL SELECT 'Personel', COUNT(*) FROM Personel
UNION ALL SELECT 'Basvuru', COUNT(*) FROM Basvuru
UNION ALL SELECT 'BasvuruDurumGecmisi', COUNT(*) FROM BasvuruDurumGecmisi;

/*
Olcum deneyleri (10.000 kisi):
  Dongu, autocommit        : 40.000 ms  (her satir ayri commit, log diske yaziliyor)
  Dongu, tek transaction   :    175 ms
  Set-based, tek INSERT    :     37 ms
*/