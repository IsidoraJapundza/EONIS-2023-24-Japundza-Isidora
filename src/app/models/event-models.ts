export interface Event {
  id_dogadjaj: string;
  nazivSportskogDogadjaja: string;
  datumOdrzavanja: string;
  vremeOdrzavanja: string;
  predvidjenoVremeZavrsetka: string;
  mestoOdrzavanja: string;
}

export interface Ticket {
  id_kontingentKarata: string;
  nazivKarte: string;
  sektor: string;
  ulaz: string;
  cena: number;
  kolicina: number;
  napomena: string;
  id_administrator: string;
  id_dogadjaj: string;
}

export interface NewEvent {
  nazivSportskogDogadjaja: string;
  datumOdrzavanja: string;
  vremeOdrzavanja: string;
  predvidjenoVremeZavrsetka: string;
  mestoOdrzavanja: string;
}

export interface NewTicket {
  nazivKarte: string;
  sektor: string;
  ulaz: string;
  cena: number;
  kolicina: number;
  napomena: string;
  id_administrator: string;
  id_dogadjaj: string;
}
