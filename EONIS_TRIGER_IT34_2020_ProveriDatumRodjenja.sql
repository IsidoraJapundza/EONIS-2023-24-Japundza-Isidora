-------------------- CREATE TRIGGER --------------------
CREATE TRIGGER ProveriDatumRodjenja
ON EONIS_IT34_2020.Porudzbina
AFTER INSERT
AS
BEGIN
	DECLARE @Id_porudzbina uniqueidentifier;
    DECLARE @Id_korisnika uniqueidentifier;
    DECLARE @Id_kontingentaKarata uniqueidentifier;
    DECLARE @DatumRodjenjaKorisnika DATE;
    DECLARE @DatumOdrzavanja DATE;
    DECLARE @OriginalnaCena INT;
    DECLARE @NovaCena INT;
    
    -- vrednosti iz tabele inserted
    SELECT @Id_porudzbina = Id_porudzbina, @Id_korisnika = Id_korisnik, @Id_kontingentaKarata = Id_kontingentKarata
    FROM inserted;
    
    -- datum rođenja korisnika
    SELECT @DatumRodjenjaKorisnika = DatumRodjenjaKorisnika
    FROM EONIS_IT34_2020.Korisnik
    WHERE Id_korisnik = @Id_korisnika;
    
    -- datum održavanja događaja i originalne cene karte
    SELECT @DatumOdrzavanja = D.DatumOdrzavanja, @OriginalnaCena = K.Cena
    FROM EONIS_IT34_2020.Dogadjaj D
    JOIN EONIS_IT34_2020.KontingentKarata K ON D.Id_dogadjaj = K.Id_dogadjaj
    WHERE K.Id_kontingentKarata = @Id_kontingentaKarata;
    
    -- Provera da li su samo dan i mesec datuma jednaki
    IF MONTH(@DatumRodjenjaKorisnika) = MONTH(@DatumOdrzavanja) AND DAY(@DatumRodjenjaKorisnika) = DAY(@DatumOdrzavanja)
    BEGIN
        -- Izračunavanje nove cene sa popustom od 10%
        SET @NovaCena = @OriginalnaCena * 0.9;
        
        -- Ažuriranje cene u tabeli Porudzbina
        UPDATE EONIS_IT34_2020.Porudzbina
        SET UkupnaCena = @NovaCena * BrojKarata
        WHERE Id_porudzbina = @Id_porudzbina AND Id_korisnik = @Id_korisnika AND Id_kontingentKarata = @Id_kontingentaKarata;
    END
END;

/*
select * from eonis_it34_2020.Dogadjaj -- 9884C2DE-3377-458F-8F57-0180FDD93BA4
select * from eonis_it34_2020.Dorisnik -- 24726427-D1C1-4C21-979E-DE7047A3C330 -- 54726427-D1C1-4C21-979E-DE7047A3E330
select * from eonis_it34_2020.Porudzbina
select * from EONIS_IT34_2020.KontingentKarata -- D74836C4-89B5-4900-82E6-D6098431E94F


-- Provera trigera ProveriDatumRodjenja
insert into eonis_it34_2020.porudzbina(id_porudzbina, datumporudzbine, vremeporudzbine, brojkarata, statusporudzbine, potvrdaplacanja, metodaisporuke, adresaisporuke, dodatnenapomene, id_korisnik, id_kontingentkarata)
values (
	'8f3a5b13-537d-444e-ae15-44d6966ee0b8',
	getdate(),
	convert(time, getdate()),
	1, 'U toku', 'Za naplatu', 'Dostava', 'Adresa 1', 'Napomena 1', '24726427-D1C1-4C21-979E-DE7047A3C330', 'D74836C4-89B5-4900-82E6-D6098431E94F'
	);

*/