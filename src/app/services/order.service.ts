import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { NewOrder, Order, PayRequest } from '../models/order-models';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/porudzbina`)
  }

  deleteOrder(id_porudzbina: string, id_korisnik: string, id_kontingentKarata: string) {
    return this.http.delete(`${this.baseUrl}/porudzbina/${id_porudzbina}/${id_korisnik}/${id_kontingentKarata}`)
  }

  createOrder(order: NewOrder): Observable<Order> {
    return this.http.post<Order>(`${this.baseUrl}/porudzbina`, order)
  }

  getUserOrders(userId: string): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/porudzbina/GetPorudzbinaByKorisnik/${userId}`)
  }

  createPayment(paymentRequest: PayRequest) {
    return this.http.post(`${this.baseUrl}/payments`, paymentRequest);
  }
}
