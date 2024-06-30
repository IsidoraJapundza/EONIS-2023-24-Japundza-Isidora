import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AdminCreationModel, AdminModel, AuthCreds, UserCreationModel, UserModel } from '../models/user-models';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  authUrl = environment.authUrl;
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  login(authCreds: AuthCreds, isAdmin: boolean) {
    return isAdmin ? this.http.post<{token: string}>(`${this.authUrl}/administrator`, authCreds) : this.http.post<{token: string}>(`${this.authUrl}/korisnik`, authCreds);
  }

  registerUser(payload: UserCreationModel) {
    return this.http.post(`${this.baseUrl}/korisnik`, payload)
  }

  registerAdmin(payload: AdminCreationModel) {
    return this.http.post(`${this.baseUrl}/administrator`, payload)
  }

  getAdmin(username: string) {
    return this.http.get<AdminModel>(`${this.baseUrl}/administrator/username/${username}`);
  }

  getUser(username: string) {
    return this.http.get<UserModel>(`${this.baseUrl}/korisnik/username/${username}`);
  }
}
