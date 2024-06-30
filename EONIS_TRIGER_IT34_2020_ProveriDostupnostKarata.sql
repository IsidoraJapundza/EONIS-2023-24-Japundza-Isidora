-------------------- CREATE TRIGGER ProveriDostupnostKarata --------------------
CREATE TRIGGER ProveriDostupnostKarata
ON EONIS_IT34_2020.Porudzbina
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @Id_porudzbina uniqueidentifier;
    DECLARE @dostupna INT;
    DECLARE @Id_kontingentKarata uniqueidentifier;
    DECLARE @BrojKarata INT;
    DECLARE @UkupnaCena INT;
    DECLARE @StatusPorudzbine VARCHAR(12);
    DECLARE @PotvrdaPlacanja VARCHAR(10);
    DECLARE @MetodaIsporuke VARCHAR(12);
    DECLARE @AdresaIsporuke VARCHAR(12);
    DECLARE @DodatneNapomene VARCHAR(50);
    DECLARE @Id_korisnik uniqueidentifier;
    DECLARE @DatumPorudzbine DATE;
    DECLARE @VremePorudzbine TIME;

    SELECT 
		@Id_porudzbina = Id_porudzbina,
        @Id_kontingentKarata = Id_kontingentKarata,
        @BrojKarata = BrojKarata,
        @UkupnaCena = UkupnaCena,
        @StatusPorudzbine = StatusPorudzbine,
        @PotvrdaPlacanja = PotvrdaPlacanja,
        @MetodaIsporuke = MetodaIsporuke,
        @AdresaIsporuke = AdresaIsporuke,
        @DodatneNapomene = DodatneNapomene,
        @Id_korisnik = Id_korisnik,
        @DatumPorudzbine = DatumPorudzbine,
        @VremePorudzbine = VremePorudzbine
    FROM inserted;

    -- dostupne količine karata za dati kontingent karata
    SELECT @dostupna = Kolicina
    FROM EONIS_IT34_2020.KontingentKarata
    WHERE Id_kontingentKarata = @Id_kontingentKarata;

    -- Provera da li je tražena količina veća od dostupne količine
    IF @BrojKarata > @dostupna
    BEGIN
        RAISERROR ('Nema dovoljno dostupnih karata za ovu porudžbinu', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        -- Ažuriranje dostupne količine karata u kontingentu
        UPDATE EONIS_IT34_2020.KontingentKarata
        SET Kolicina = Kolicina - @BrojKarata
        WHERE Id_kontingentKarata = @Id_kontingentKarata;

        -- Umetanje nove porudžbine
        INSERT INTO EONIS_IT34_2020.Porudzbina
        (
            Id_porudzbina, DatumPorudzbine, VremePorudzbine, BrojKarata, UkupnaCena, StatusPorudzbine,
            PotvrdaPlacanja, MetodaIsporuke, AdresaIsporuke, DodatneNapomene,
            Id_korisnik, Id_kontingentKarata
        )
        VALUES
        (
            @Id_porudzbina, @DatumPorudzbine, @VremePorudzbine, @BrojKarata, @UkupnaCena, @StatusPorudzbine,
            @PotvrdaPlacanja, @MetodaIsporuke, @AdresaIsporuke, @DodatneNapomene,
            @Id_korisnik, @Id_kontingentKarata
        );
    END
END;

/*
select * from EONIS_IT34_2020.Korisnik
select * from EONIS_IT34_2020.Kontingentkarata
Select * from EONIS_IT34_2020.Porudzbina
SELECT * from EONIS_IT34_2020.Dogadjaj

-- Provera trigera ProveriDostupnostKarata i ProveriDatumRodjenja
INSERT INTO EONIS_IT34_2020.Porudzbina(id_porudzbina, DatumPorudzbine, VremePorudzbine, BrojKarata, UkupnaCena, StatusPorudzbine, PotvrdaPlacanja, MetodaIsporuke, AdresaIsporuke, DodatneNapomene, Id_korisnik, Id_kontingentKarata)
	SELECT
		'8f3a5b13-537d-444e-ae15-44d6966ee0b8', GETDATE(), SYSDATETIME(), 2, KontingentKarata.Cena * 2, 'U toku', 'Za naplatu', 'Pdf mejl', 'Adresa1', 'Napomena1', '54726427-D1C1-4C21-979E-DE7047A3E330', 'D74836C4-89B5-4900-82E6-D6098431E94F'
FROM 
    EONIS_IT34_2020.KontingentKarata KontingentKarata
WHERE 
    KontingentKarata.Id_kontingentKarata = 'D74836C4-89B5-4900-82E6-D6098431E94F';
	
*/

