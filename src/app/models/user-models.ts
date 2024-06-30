export interface AuthCreds {
  korisnickoIme: string,
  lozinka: string
}

export interface AdminCreationModel {
  ImeAdministratora: string;
  PrezimeAdministratora: string;
  KorisnickoImeAdministratora: string;
  MejlAdministratora: string;
  LozinkaAdministratora?: string;
  KontaktAdministratora: string;
  StatusAktivnosti?: string;
  Privilegije?: string;
}

export interface AdminModel {
  id_administrator: string;
  imeAdministratora: string;
  prezimeAdministratora: string;
  korisnickoImeAdministratora: string;
  mejlAdministratora: string;
  lozinkaAdministratora?: string;
  kontaktAdministratora: string;
  statusAktivnosti?: string;
  privilegije?: string;
}

export interface UserCreationModel {
  ImeKorisnika: string;
  PrezimeKorisnika: string;
  KorisnickoImeKorisnika: string;
  LozinkaKorisnika?: string;
  MejlKorisnika: string;
  KontaktKorisnika: string;
  AdresaKorisnika?: string;
  PrebivalisteKorisnika?: string;
  PostanskiBroj?: number;
  DatumRodjenjaKorisnika: string;
}

export interface RegistrationFormModel {
  name: string,
  lastName: string,
  userName: string,
  password: string,
  email: string,
  phone: string,
  address?: string,
  homeTown?: string,
  postalCode?: string,
  dateOfBirth: Date
}

export interface UserModel {
  id_korisnik: string;
  imeKorisnika: string;
  prezimeKorisnika: string;
  korisnickoImeKorisnika: string;
  mejlKorisnika: string;
  kontaktKorisnika: string;
  adresaKorisnika?: string;
  prebivalisteKorisnika?: string;
  postanskiBroj?: number;
  datumRodjenjaKorisnika: string; // You might need to adjust the type based on your actual data
  datumRegistracijeKorisnika: string; // Same here
}
