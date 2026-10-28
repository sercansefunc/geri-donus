USE master;

DROP DATABASE IF EXISTS GeriDonus;

CREATE DATABASE GeriDonus;

GO

USE GeriDonus;

GO

CREATE TABLE Aile (
    id  int  IDENTITY(1,1),
    CONSTRAINT PK_Aile PRIMARY KEY (id)
);
CREATE TABLE Il (
	id int IDENTITY(1,1),
	ad nvarchar(20) NOT NULL,
	CONSTRAINT PK_Il PRIMARY KEY (id)
);
CREATE TABLE Durum (
	id int IDENTITY(1,1),
	ad nvarchar(30) NOT NULL,
	CONSTRAINT PK_Durum PRIMARY KEY (id)
);
CREATE TABLE Personel (
	id int IDENTITY(1,1),
	il_id int NOT NULL,
	ad nvarchar(50) NOT NULL,
	soyad nvarchar(50) NOT NULL,
	rol nvarchar(20) NOT NULL,
	CONSTRAINT PK_Personel PRIMARY KEY (id),
	CONSTRAINT FK_Personel_Il FOREIGN KEY (il_id) REFERENCES Il(id)
);
CREATE TABLE Kisi (
	id int IDENTITY(1,1),
	aile_id int NOT NULL,
	il_id int NOT NULL,
	kimlikno varchar(20) NULL,
	ad nvarchar(50) NOT NULL,
	soyad nvarchar(50) NOT NULL,
	adres nvarchar(200) NULL,
	telno varchar(20) NULL,
	dogumtarihi date NOT NULL,
	CONSTRAINT PK_Kisi PRIMARY KEY(id),
	CONSTRAINT FK_Kisi_Aile FOREIGN KEY (aile_id) REFERENCES Aile(id),
	CONSTRAINT FK_Kisi_Il FOREIGN KEY (il_id) REFERENCES Il(id)
);
CREATE TABLE Basvuru (
	id int IDENTITY(1,1),
	kisi_id int NOT NULL,
	mudurluk_il_id int NOT NULL,
	basvurutarihi date NOT NULL,
	CONSTRAINT PK_Basvuru PRIMARY KEY (id),
	CONSTRAINT FK_Basvuru_Kisi FOREIGN KEY(kisi_id) REFERENCES Kisi(id),
	CONSTRAINT FK_Basvuru_Il FOREIGN KEY(mudurluk_il_id) REFERENCES Il(id)
);
CREATE TABLE BasvuruDurumGecmisi (
	id int IDENTITY(1,1),
	basvuru_id int NOT NULL,
	durum_id int NOT NULL,
	personel_id int NOT NULL,
	islemzamani datetime2(0) NOT NULL,
	gerekce nvarchar(500) NULL,
	CONSTRAINT PK_BasvuruDurumGecmisi PRIMARY KEY (id),
	CONSTRAINT FK_BasvuruDurumGecmisi_Basvuru FOREIGN KEY (basvuru_id) REFERENCES Basvuru(id),
	CONSTRAINT FK_BasvuruDurumGecmisi_Durum FOREIGN KEY (durum_id) REFERENCES Durum(id),
	CONSTRAINT FK_BasvuruDurumGecmisi_Personel FOREIGN KEY (personel_id) REFERENCES Personel(id)
);





-- Benzersizlik kısıtları
ALTER TABLE Il
ADD CONSTRAINT UQ_Il_ad UNIQUE (ad);

ALTER TABLE Durum
ADD CONSTRAINT UQ_Durum_ad UNIQUE (ad);

-- Kimlik numarası: boş olabilir, doluysa benzersiz olmalı
CREATE UNIQUE INDEX UX_Kisi_kimlikno
ON Kisi (kimlikno)
WHERE kimlikno IS NOT NULL;

-- Varsayılan zamanlar
ALTER TABLE BasvuruDurumGecmisi
ADD CONSTRAINT DF_BasvuruDurumGecmisi_islemzamani
DEFAULT SYSDATETIME() FOR islemzamani;

ALTER TABLE Basvuru
ADD CONSTRAINT DF_Basvuru_basvurutarihi
DEFAULT SYSDATETIME() FOR basvurutarihi;

-- Doğum tarihi gelecekte olamaz
ALTER TABLE Kisi
ADD CONSTRAINT CK_Kisi_dogumtarihi
CHECK (CAST(SYSDATETIME() AS DATE) >= dogumtarihi);