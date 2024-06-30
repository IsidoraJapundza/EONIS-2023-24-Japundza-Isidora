							------------------------------ IT 34/2020 ------------------------------
------------------------------ DDL - script za kreiranje i brisanje schema, sekvenci i tabela ------------------------------
use EONIS_IT34_2020;
-------------------- DROP TRIGGER --------------------
IF OBJECT_ID('ProveriDostupnostKarata', 'TR') IS NOT NULL
	DROP TRIGGER ProveriDostupnostKarata;

IF OBJECT_ID('ProveriDatumRodjenja', 'TR') IS NOT NULL
	DROP TRIGGER ProveriDatumRodjenja;

-------------------- DROP TABLE --------------------
IF OBJECT_ID('EONIS_IT34_2020.Porudzbina', 'U') IS NOT NULL
	DROP TABLE EONIS_IT34_2020.Porudzbina;
GO 

IF OBJECT_ID('EONIS_IT34_2020.Korisnik', 'U') IS NOT NULL
	DROP TABLE EONIS_IT34_2020.Korisnik;
GO 

IF OBJECT_ID('EONIS_IT34_2020.KontingentKarata', 'U') IS NOT NULL
	DROP TABLE EONIS_IT34_2020.KontingentKarata;
GO 

IF OBJECT_ID('EONIS_IT34_2020.Dogadjaj', 'U') IS NOT NULL
	DROP TABLE EONIS_IT34_2020.Dogadjaj;
GO 

IF OBJECT_ID('EONIS_IT34_2020.Administrator', 'U') IS NOT NULL
	DROP TABLE EONIS_IT34_2020.Administrator;
GO 

-------------------- DROP SCHEMA --------------------
IF SCHEMA_ID('EONIS_IT34_2020') IS NOT NULL
	DROP SCHEMA EONIS_IT34_2020;
GO

-------------------- CREATE SCHEMA --------------------
GO
CREATE SCHEMA EONIS_IT34_2020;
GO

-------------------- CREATE TABLE -------------------- 
CREATE TABLE EONIS_IT34_2020.Korisnik
(
	Id_korisnik uniqueidentifier NOT NULL,
	ImeKorisnika VARCHAR(12) NOT NULL,
	PrezimeKorisnika VARCHAR(20) NOT NULL,
	KorisnickoImeKorisnika VARCHAR(30) NOT NULL,
	LozinkaKorisnikaHashed VARBINARY(MAX),
	--saltKorisnika VARCHAR(30),
	MejlKorisnika VARCHAR(30) NOT NULL,
	KontaktKorisnika VARCHAR(14) NOT NULL,
	AdresaKorisnika VARCHAR(30),
	PrebivalisteKorisnika VARCHAR(15),
	PostanskiBroj Int,
	DatumRodjenjaKorisnika Date NOT NULL,
	DatumRegistracijeKorisnika Date NOT NULL CONSTRAINT DFT_Korisnik_DatumRegistracije DEFAULT(GETDATE())
	CONSTRAINT PK_Korisnik PRIMARY KEY (Id_korisnik),
	CONSTRAINT CHK_Korisnik_PostanskiBroj CHECK (len(PostanskiBroj) = 5),
	CONSTRAINT UC_Korisnik_KorisnickoImeKorisnika UNIQUE (KorisnickoImeKorisnika),
	CONSTRAINT UC_Korisnik_MejlKorisnika UNIQUE (MejlKorisnika)
);

-- Administrator select * from eonis_it34_2020.administrator
CREATE TABLE EONIS_IT34_2020.Administrator
(
	Id_administrator uniqueidentifier NOT NULL,
	ImeAdministratora VARCHAR(12) NOT NULL,
	PrezimeAdministratora VARCHAR(20) NOT NULL,
	KorisnickoImeAdministratora VARCHAR(30) NOT NULL,
	MejlAdministratora VARCHAR(30) NOT NULL,
	LozinkaAdministratoraHashed VARBINARY(MAX),
	--saltAdministratora VARCHAR(30), -- varbinary?
	KontaktAdministratora VARCHAR(14) NOT NULL,
	StatusAktivnosti VARCHAR(10),
	Privilegije VARCHAR(50),
	CONSTRAINT PK_Administrator PRIMARY KEY (Id_administrator),
	CONSTRAINT UC_Administrator_KorisnickoImeAdministratora UNIQUE (KorisnickoImeAdministratora),
	CONSTRAINT CHK_Administrator_StatusAktivnosti CHECK (StatusAktivnosti in ('Neaktivan', 'Aktivan')),
	CONSTRAINT UC_Administrator_MejlAdministratora UNIQUE (MejlAdministratora)
);

-- Događaj select * from eonis_it34_2020.dogadjaj
CREATE TABLE EONIS_IT34_2020.Dogadjaj
(
	Id_dogadjaj uniqueidentifier NOT NULL,
	NazivSportskogDogadjaja VARCHAR(70) NOT NULL,
	DatumOdrzavanja Date,
	VremeOdrzavanja Time,
	PredvidjenoVremeZavrsetka Time,
	MestoOdrzavanja VARCHAR(30) NOT NULL,
	CONSTRAINT PK_Dogadjaj PRIMARY KEY (Id_dogadjaj)
);

-- KontingentKarata select * from eonis_it34_2020.kontingentkarata
CREATE TABLE EONIS_IT34_2020.KontingentKarata
(
	Id_kontingentKarata uniqueidentifier NOT NULL,
	NazivKarte VARCHAR(30) NOT NULL,
	Sektor VARCHAR(10) NOT NULL,
	Ulaz VARCHAR(8) NOT NULL,
	Cena int NOT NULL,
	Kolicina int NOT NULL,
	Napomena VARCHAR(100) not null,
	Id_administrator uniqueidentifier NOT NULL,
	Id_dogadjaj uniqueidentifier NOT NULL,
	CONSTRAINT PK_KontingentKarata PRIMARY KEY (Id_kontingentKarata),
	CONSTRAINT FK_KontingentKarata_Administrator FOREIGN KEY (Id_administrator)
		REFERENCES EONIS_IT34_2020.Administrator(Id_administrator), 
	CONSTRAINT FK_KontingentKarata_Dogadjaj FOREIGN KEY (Id_dogadjaj)
		REFERENCES EONIS_IT34_2020.Dogadjaj(Id_dogadjaj)  ON DELETE CASCADE --?
);

-- Porudžbina
CREATE TABLE EONIS_IT34_2020.Porudzbina
(
	Id_porudzbina uniqueidentifier NOT NULL,
	DatumPorudzbine Date NOT NULL CONSTRAINT DFT_Porudzbina_DatumPorudzbine DEFAULT(GETDATE()),
	VremePorudzbine Time NOT NULL CONSTRAINT DFT_Porudzbina_VremePorudzbine DEFAULT(SYSDATETIME()),
	BrojKarata int NOT NULL,
	UkupnaCena int,
	StatusPorudzbine VARCHAR(12) NOT NULL,
	PotvrdaPlacanja VARCHAR(10),
	MetodaIsporuke VARCHAR(12),
	AdresaIsporuke VARCHAR(12),
	DodatneNapomene VARCHAR(50),
	Id_korisnik uniqueidentifier NOT NULL,
	Id_kontingentKarata uniqueidentifier NOT NULL,
	CONSTRAINT PK_Porudzbina PRIMARY KEY (Id_porudzbina, Id_korisnik, Id_kontingentKarata),
	--CONSTRAINT PK_Porudzbina PRIMARY KEY (Id_korisnik, Id_kontingentKarata),
	CONSTRAINT CHK_Porudzbina_StatusPorudzbine CHECK (StatusPorudzbine in ('U toku', 'Završena', 'Otkazana')), --
	CONSTRAINT CHK_Porudzbina_PotvrdaPlacanja CHECK (PotvrdaPlacanja in ('Za naplatu', 'Plaćeno')), --
	CONSTRAINT FK_Porudzbina_Korisnik FOREIGN KEY (Id_korisnik)
		REFERENCES EONIS_IT34_2020.Korisnik(Id_korisnik) ON DELETE CASCADE,
	CONSTRAINT FK_Porudzbina_KontingentKarata FOREIGN KEY (Id_kontingentKarata)
		REFERENCES EONIS_IT34_2020.KontingentKarata(Id_kontingentKarata) ON DELETE CASCADE
);


							------------------------------ IT 34/2020 ------------------------------
------------------------------ DML - script za insert (unos) podataka ------------------------------
--USE it34g2020

-------------------- SELECT CHECK --------------------
/*
SELECT * FROM EONIS_IT34_2020.Porudzbina
SELECT * FROM EONIS_IT34_2020.Korisnik
SELECT * FROM EONIS_IT34_2020.KontingentKarata
SELECT * FROM EONIS_IT34_2020.Dogadjaj
SELECT * FROM EONIS_IT34_2020.Administrator
*/

-------------------- DELETE FROM TABLE --------------------
/*
DELETE FROM EONIS_IT34_2020.KontingentKarata
DELETE FROM EONIS_IT34_2020.Administrator
DELETE FROM EONIS_IT34_2020.Porudzbina
DELETE FROM EONIS_IT34_2020.Korisnik
DELETE FROM EONIS_IT34_2020.Dogadjaj
*/

-------------------- INSERT INTO TABLE --------------------
INSERT INTO EONIS_IT34_2020.Korisnik(Id_korisnik, ImeKorisnika, PrezimeKorisnika, KorisnickoImeKorisnika, LozinkaKorisnikaHashed, --saltKorisnika
									 MejlKorisnika, KontaktKorisnika, AdresaKorisnika, PrebivalisteKorisnika, 
									 PostanskiBroj, DatumRodjenjaKorisnika, DatumRegistracijeKorisnika)
	VALUES (
		'24726427-d1c1-4c21-979e-de7047a3c330',
		'Luka',
		'Japundža',
		'lukajapundza',
		(CONVERT(VARBINARY(MAX), '48656C6C6F20576F726C64', 2)),
		'luka.japundza@gmail.com',
		'060456789',
		'Adi Endrea 1/4',
		'Temerin',
		21235,
		convert(date, '2005-03-19', 23),
		GETDATE()
	)

INSERT INTO EONIS_IT34_2020.Korisnik(Id_korisnik, ImeKorisnika, PrezimeKorisnika, KorisnickoImeKorisnika, LozinkaKorisnikaHashed, MejlKorisnika, KontaktKorisnika, AdresaKorisnika, PrebivalisteKorisnika, PostanskiBroj, DatumRodjenjaKorisnika, DatumRegistracijeKorisnika)
	VALUES (
		'280d09bb-dc29-433a-abf4-e094863e6ae5',
		'Marija',
		'Babić',
		'marijababic',
		(CONVERT(VARBINARY(MAX), '48656C6C6F20576F726C64', 2)),
		--'u3s9^&lqP@!Fz#',
		'marija.babic@gmail.com',
		'061234567',
		'Novosadska 254',
		'Temerin',
		21235,
		convert(date, '2000-05-17', 23),
		GETDATE()
	)

INSERT INTO EONIS_IT34_2020.Korisnik(Id_korisnik, ImeKorisnika, PrezimeKorisnika, KorisnickoImeKorisnika, LozinkaKorisnikaHashed, MejlKorisnika, KontaktKorisnika, AdresaKorisnika, PrebivalisteKorisnika, PostanskiBroj, DatumRodjenjaKorisnika, DatumRegistracijeKorisnika)
	VALUES (
		'234ea8a1-3a14-4fc3-8d3b-e0de892680b5',
		'Aleksandra',
		'Marković',
		'aleksandramarkovic',
		(CONVERT(VARBINARY(MAX), '48656C6C6F20576F726C64', 2)),
		--'u4s9^&lqP@!Fz#',
		'aleksandra.markovic@gmail.com',
		'061347869',
		'Temerinska 58',
		'Novi Sad',
		21000,
		convert(date, '1998-04-17', 23),
		GETDATE()
	)

INSERT INTO EONIS_IT34_2020.Korisnik(Id_korisnik, ImeKorisnika, PrezimeKorisnika, KorisnickoImeKorisnika, LozinkaKorisnikaHashed, MejlKorisnika, KontaktKorisnika, AdresaKorisnika, PrebivalisteKorisnika, PostanskiBroj, DatumRodjenjaKorisnika, DatumRegistracijeKorisnika)
	VALUES (
		'445b10cd-26bd-4203-aa4a-14798ebe321f',
		'Ela',
		'Jovanović',
		'elajovanovic',
		(CONVERT(VARBINARY(MAX), '48656C6C6F20576F726C64', 2)),
		--'u5s9^&lqP@!Fz#',
		'ela.jovanovic@gmail.com',
		'069458264',
		'Balzakova 60/5',
		'Novi Sad',
		21000,
		convert(date, '2001-03-25', 23),
		GETDATE()
	)

INSERT INTO EONIS_IT34_2020.Korisnik(Id_korisnik, ImeKorisnika, PrezimeKorisnika, KorisnickoImeKorisnika, LozinkaKorisnikaHashed, MejlKorisnika, KontaktKorisnika, AdresaKorisnika, PrebivalisteKorisnika, PostanskiBroj, DatumRodjenjaKorisnika, DatumRegistracijeKorisnika)
	VALUES (
		'2e082c26-a7f3-466f-9cd1-d17bf44b1226',
		'Marko',
		'Kostić',
		'markokostic',
		(CONVERT(VARBINARY(MAX), '48656C6C6F20576F726C64', 2)),
		--'u6s9^&lqP@!Fz#',
		'marko.kostic@gmail.com',
		'068549125',
		'Fruškogorska 129',
		'Novi Sad',
		21000,
		convert(date, '2002-01-19', 23),
		GETDATE()
	)

-- EONIS_IT34_2020.Korisnik --
INSERT INTO EONIS_IT34_2020.Korisnik(Id_korisnik, ImeKorisnika, PrezimeKorisnika, KorisnickoImeKorisnika, LozinkaKorisnikaHashed, --saltKorisnika
									 MejlKorisnika, KontaktKorisnika, AdresaKorisnika, PrebivalisteKorisnika, 
									 PostanskiBroj, DatumRodjenjaKorisnika, DatumRegistracijeKorisnika)
	VALUES (
		'54726427-d1c1-4c21-979e-de7047a3e330',
		'Andrej',
		'Milosevic',
		'andrejmilosevic',
		(CONVERT(VARBINARY(MAX), '48656C6C6F20576F726C64', 2)),
		--'u2s9^&lqP@!Fz#',
		'andrej.milosevic@gmail.com',
		'060456789',
		'Novosadska 359',
		'Temerin',
		21235,
		convert(date, '2000-03-19', 23),
		GETDATE()
	)

-- EONIS_IT34_2020.Administrator --
INSERT INTO EONIS_IT34_2020.Administrator(Id_administrator, ImeAdministratora, PrezimeAdministratora, KorisnickoImeAdministratora, MejlAdministratora, LozinkaAdministratoraHashed, KontaktAdministratora, StatusAktivnosti, Privilegije)
	VALUES (
		'baa23175-ac44-4bbf-8359-56dd732721f8',
		'Isidora',
		'Japundža',
		'isidorajapundza',
		'isidora10@gmail.com',
		(CONVERT(VARBINARY(MAX), '48656C6C6F20576F726C64', 2)),
		--'u6s9gjlqPfjFz4',
		'0600823712',
		'Aktivan',
		'Upravljanje kartama'
	)

INSERT INTO EONIS_IT34_2020.Administrator(Id_administrator, ImeAdministratora, PrezimeAdministratora, KorisnickoImeAdministratora, MejlAdministratora, LozinkaAdministratoraHashed, KontaktAdministratora, StatusAktivnosti, Privilegije)
	VALUES (
		'13e83089-f661-41b9-8d44-ed73d158e44d',
		'Aleksej',
		'Markanović',
		'aleksejmarkanovic97',
		'aleksej@gmail.com',
		(CONVERT(VARBINARY(MAX), '48656C6C6F20576F726C64', 2)),
		--'u6s9^&lqP@!Fz#',
		'0600823712',
		'Aktivan',
		'Upravljanje kartama i podrška'
	)

-- EONIS_IT34_2020.Dogadjaj --
INSERT INTO EONIS_IT34_2020.Dogadjaj(Id_dogadjaj, NazivSportskogDogadjaja, DatumOdrzavanja, VremeOdrzavanja, PredvidjenoVremeZavrsetka, MestoOdrzavanja)
	VALUES (
		'ada35a96-f159-4d32-9c12-669fa152943b',
		'KK Partizan Mozzart Bet vs KK FMP',
		convert(date, '2024-03-16', 23),
		'21:00:00',
		'23:00:00',
		'Štark Arena'
	)

	
INSERT INTO EONIS_IT34_2020.Dogadjaj(Id_dogadjaj, NazivSportskogDogadjaja, DatumOdrzavanja, VremeOdrzavanja, PredvidjenoVremeZavrsetka, MestoOdrzavanja)
	VALUES (
		'9884c2de-3377-458f-8f57-0180fdd93ba4',
		'KK Partizan Mozzart Bet vs Baskonia',
		convert(date, '2024-03-19', 23),
		'20:30:00',
		'22:30:00',
		'Štark Arena'
	)

INSERT INTO EONIS_IT34_2020.Dogadjaj(Id_dogadjaj, NazivSportskogDogadjaja, DatumOdrzavanja, VremeOdrzavanja, PredvidjenoVremeZavrsetka, MestoOdrzavanja)
	VALUES (
		'84469cf0-3185-46e2-95a8-0d02c349e0b8',
		'KK Partizan Mozzart Bet vs Real Madrid',
		convert(date, '2024-03-21', 23),
		'20:30:00',
		'22:30:00',
		'Štark Arena'
	)

INSERT INTO EONIS_IT34_2020.Dogadjaj(Id_dogadjaj, NazivSportskogDogadjaja, DatumOdrzavanja, VremeOdrzavanja, PredvidjenoVremeZavrsetka, MestoOdrzavanja)
	VALUES (
		'92e0c6e6-f4a5-44a4-98e8-31058a7878ae',
		'KK Partizan Mozzart Bet vs Olympiacos',
		convert(date, '2024-03-24', 23),
		'20:30:00',
		'22:30:00',
		'Štark Arena'
	)

INSERT INTO EONIS_IT34_2020.Dogadjaj(Id_dogadjaj, NazivSportskogDogadjaja, DatumOdrzavanja, VremeOdrzavanja, PredvidjenoVremeZavrsetka, MestoOdrzavanja)
	VALUES (
		'827f209b-e322-458f-a589-8aa21a59d07d',
		'KK Partizan Mozzart Bet vs Zadar',
		convert(date, '2024-04-06', 23),
		'18:00:00',
		'20:00:00',
		'Štark Arena'
	)

INSERT INTO EONIS_IT34_2020.Dogadjaj(Id_dogadjaj, NazivSportskogDogadjaja, DatumOdrzavanja, VremeOdrzavanja, PredvidjenoVremeZavrsetka, MestoOdrzavanja)
	VALUES (
		'b887a92b-9267-40a3-bff6-6d9e005ae40f',
		'FK Partizan vs Spartak Subotica',
		convert(date, '2024-03-30', 23),
		'16:00:00',
		'18:00:00',
		'Stadion Partizana (Beograd)'
	)

INSERT INTO EONIS_IT34_2020.Dogadjaj(Id_dogadjaj, NazivSportskogDogadjaja, DatumOdrzavanja, VremeOdrzavanja, PredvidjenoVremeZavrsetka, MestoOdrzavanja)
	VALUES (
		'1f3ba959-649a-42d7-b1dd-fe0499a994fe',
		'KK Partizan Mozzart Bet vs Valencia',
		convert(date, '2024-04-12', 23),
		'20:30:00',
		'22:30:00',
		'Štark Arena'
	)

-- EONIS_IT34_2020.KontingentKarata --
INSERT INTO EONIS_IT34_2020.KontingentKarata(Id_kontingentKarata, NazivKarte, Sektor, Ulaz, Cena, Kolicina, Napomena, Id_administrator, Id_dogadjaj)
	VALUES (
		'a59003cc-9c09-4b03-84ac-5091ea2b869f',
		'EuroLeague',
		'427',
		'SEVER',
		800,
		50,
		'/',
		'baa23175-ac44-4bbf-8359-56dd732721f8',
		'ada35a96-f159-4d32-9c12-669fa152943b'
	)

INSERT INTO EONIS_IT34_2020.KontingentKarata(Id_kontingentKarata, NazivKarte, Sektor, Ulaz, Cena, Kolicina, Napomena, Id_administrator, Id_dogadjaj)
	VALUES (
		'd74836c4-89b5-4900-82e6-d6098431e94f',
		'EuroLeague',
		'216',
		'SEVER',
		6500,
		3,
		'/',
		'baa23175-ac44-4bbf-8359-56dd732721f8',
		'9884c2de-3377-458f-8f57-0180fdd93ba4'
	)

INSERT INTO EONIS_IT34_2020.KontingentKarata(Id_kontingentKarata, NazivKarte, Sektor, Ulaz, Cena, Kolicina, Napomena, Id_administrator, Id_dogadjaj)
	VALUES (
		'5a3ae431-8e2c-4658-9f2f-b83f8e7691e2',
		'EuroLeague',
		'205',
		'JUG',
		7500,
		12,
		'Vikend',
		'baa23175-ac44-4bbf-8359-56dd732721f8',
		'84469cf0-3185-46e2-95a8-0d02c349e0b8'
	)


INSERT INTO EONIS_IT34_2020.KontingentKarata(Id_kontingentKarata, NazivKarte, Sektor, Ulaz, Cena, Kolicina, Napomena, Id_administrator, Id_dogadjaj)
	VALUES (
		'7ece1254-af56-4355-ad3a-59f179bf0b2a',
		'EuroLeague',
		'422',
		'SEVER',
		2500,
		7,
		'/',
		'baa23175-ac44-4bbf-8359-56dd732721f8',
		'92e0c6e6-f4a5-44a4-98e8-31058a7878ae'
	)

INSERT INTO EONIS_IT34_2020.KontingentKarata(Id_kontingentKarata, NazivKarte, Sektor, Ulaz, Cena, Kolicina, Napomena, Id_administrator, Id_dogadjaj)
	VALUES (
		'a01babb3-50bc-4334-89ab-daaea7f2b870',
		'Aba League',
		'202',
		'JUG',
		1500,
		3,
		'/',
		'13e83089-f661-41b9-8d44-ed73d158e44d',
		'827f209b-e322-458f-a589-8aa21a59d07d'
	)


INSERT INTO EONIS_IT34_2020.KontingentKarata(Id_kontingentKarata, NazivKarte, Sektor, Ulaz, Cena, Kolicina, Napomena, Id_administrator, Id_dogadjaj)
	VALUES (
		'338eaaa0-22f0-40d4-834b-097f7c609917',
		'Super Liga',
		'Istok',
		'M',
		800,
		10,
		'/',
		'13e83089-f661-41b9-8d44-ed73d158e44d',
		'b887a92b-9267-40a3-bff6-6d9e005ae40f'
	)

INSERT INTO EONIS_IT34_2020.KontingentKarata(Id_kontingentKarata, NazivKarte, Sektor, Ulaz, Cena, Kolicina, Napomena, Id_administrator, Id_dogadjaj)
	VALUES (
		'258d8a31-b0a2-46f1-b61a-a358a1db9760',
		'EuroLeague',
		'422',
		'SEVER',
		1800,
		10,
		'/',
		'13e83089-f661-41b9-8d44-ed73d158e44d',
		'1f3ba959-649a-42d7-b1dd-fe0499a994fe'
	)


-- EONIS_IT34_2020.Porudzbina 
INSERT INTO EONIS_IT34_2020.Porudzbina(Id_porudzbina, DatumPorudzbine, VremePorudzbine, BrojKarata, UkupnaCena, StatusPorudzbine, PotvrdaPlacanja, MetodaIsporuke, AdresaIsporuke, DodatneNapomene, Id_korisnik, Id_kontingentKarata)
	SELECT
		'222bc786-be76-4a18-a7ca-c7ac6e893858', GETDATE(), SYSDATETIME(), 1, KontingentKarata.Cena * 1, 'U toku', 'Za naplatu', 'Pdf mejl', '/', '/', '24726427-d1c1-4c21-979e-de7047a3c330', 'a59003cc-9c09-4b03-84ac-5091ea2b869f'
FROM 
    EONIS_IT34_2020.KontingentKarata KontingentKarata
WHERE 
    KontingentKarata.Id_kontingentKarata = 'a59003cc-9c09-4b03-84ac-5091ea2b869f';
 


INSERT INTO EONIS_IT34_2020.Porudzbina(Id_porudzbina, DatumPorudzbine, VremePorudzbine, BrojKarata, UkupnaCena, StatusPorudzbine, PotvrdaPlacanja, MetodaIsporuke, AdresaIsporuke, DodatneNapomene, Id_korisnik, Id_kontingentKarata)
	SELECT
		'378872a6-3fcd-48a4-a8c9-a50668597607', GETDATE(), SYSDATETIME(), 5, KontingentKarata.Cena * 5, 'Završena', 'Plaćeno', 'Pdf mejl', '/', '/', '280d09bb-dc29-433a-abf4-e094863e6ae5', 'A59003CC-9C09-4B03-84AC-5091EA2B869F'
FROM 
    EONIS_IT34_2020.KontingentKarata KontingentKarata
WHERE 
    KontingentKarata.Id_kontingentKarata = 'A59003CC-9C09-4B03-84AC-5091EA2B869F';
 

INSERT INTO EONIS_IT34_2020.Porudzbina(Id_porudzbina, DatumPorudzbine, VremePorudzbine, BrojKarata, UkupnaCena, StatusPorudzbine, PotvrdaPlacanja, MetodaIsporuke, AdresaIsporuke, DodatneNapomene, Id_korisnik, Id_kontingentKarata)
	SELECT
		'3cfbfb9d-ee13-4c19-8137-7521193d0988', GETDATE(), SYSDATETIME(), 2, KontingentKarata.Cena * 2, 'Otkazana', 'Za naplatu', '/', '/', '/', '234ea8a1-3a14-4fc3-8d3b-e0de892680b5', '5a3ae431-8e2c-4658-9f2f-b83f8e7691e2'
FROM 
    EONIS_IT34_2020.KontingentKarata KontingentKarata
WHERE 
    KontingentKarata.Id_kontingentKarata = '5a3ae431-8e2c-4658-9f2f-b83f8e7691e2';


INSERT INTO EONIS_IT34_2020.Porudzbina(Id_porudzbina, DatumPorudzbine, VremePorudzbine, BrojKarata, UkupnaCena, StatusPorudzbine, PotvrdaPlacanja, MetodaIsporuke, AdresaIsporuke, DodatneNapomene, Id_korisnik, Id_kontingentKarata)
	SELECT
		'5a8bc89e-54ab-4c77-a2c0-ba9f759d7650', GETDATE(), SYSDATETIME(), 3, KontingentKarata.Cena * 3, 'U toku', 'Za naplatu', 'Pdf mejl', '/', '/', '445b10cd-26bd-4203-aa4a-14798ebe321f', '7ece1254-af56-4355-ad3a-59f179bf0b2a'
FROM 
    EONIS_IT34_2020.KontingentKarata KontingentKarata
WHERE 
    KontingentKarata.Id_kontingentKarata = '7ece1254-af56-4355-ad3a-59f179bf0b2a';


INSERT INTO EONIS_IT34_2020.Porudzbina(Id_porudzbina, DatumPorudzbine, VremePorudzbine, BrojKarata, UkupnaCena, StatusPorudzbine, PotvrdaPlacanja, MetodaIsporuke, AdresaIsporuke, DodatneNapomene, Id_korisnik, Id_kontingentKarata)
	SELECT
		'8f665537-6c12-448e-be0e-b90fb13155c9', GETDATE(), SYSDATETIME(), 3, KontingentKarata.Cena * 3, 'U toku', 'Za naplatu', 'Pdf mejl', '/', '/', '2e082c26-a7f3-466f-9cd1-d17bf44b1226', 'a01babb3-50bc-4334-89ab-daaea7f2b870'
FROM 
    EONIS_IT34_2020.KontingentKarata KontingentKarata
WHERE 
    KontingentKarata.Id_kontingentKarata = 'a01babb3-50bc-4334-89ab-daaea7f2b870';



select * from EONIS_IT34_2020.Administrator
select * from EONIS_IT34_2020.Korisnik
select * from EONIS_IT34_2020.KontingentKarata
select * from EONIS_IT34_2020.Dogadjaj
select * from EONIS_IT34_2020.Porudzbina
