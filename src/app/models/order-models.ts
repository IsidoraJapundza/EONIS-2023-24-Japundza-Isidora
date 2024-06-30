export interface Order {
  id_porudzbina: string;
  datumPorudzbine: string;
  vremePorudzbine: string;
  brojKarata: number;
  ukupnaCena: number;
  statusPorudzbine: string;
  potvrdaPlacanja?: string;
  metodaIsporuke?: string;
  adresaIsporuke?: string;
  dodatneNapomene?: string;
  id_korisnik: string;
  id_kontingentKarata: string;
}

export interface NewOrder {
  brojKarata: number;
  statusPorudzbine: string;
  potvrdaPlacanja?: string;
  metodaIsporuke?: string;
  adresaIsporuke?: string;
  dodatneNapomene?: string;
  id_korisnik: string;
  id_kontingentKarata: string;
}

export interface PayRequest {
  paymentMethodId: string;
  price: number;
  orderId: string;
}
