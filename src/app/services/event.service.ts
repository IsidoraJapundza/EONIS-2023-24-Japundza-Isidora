import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Event, NewEvent, NewTicket, Ticket } from 'src/app/models/event-models';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getAllEvents(pageSize?: number, pageNumber?: number): Observable<Event[]> {
    if(pageSize && pageNumber) {
      const params = new HttpParams()
      .set('pageSize', pageSize.toString())
      .set('page', pageNumber.toString());

      return this.http.get<Event[]>(`${this.baseUrl}/dogadjaj`, {params})
    } else {
      return this.http.get<Event[]>(`${this.baseUrl}/dogadjaj`)
    }

  }

  getAllTickets(): Observable<Ticket[]> {

    return this.http.get<Ticket[]>(`${this.baseUrl}/kontingentKarata`)
  }

  createEvent(event: NewEvent) {
    return this.http.post<Event>(`${this.baseUrl}/dogadjaj`, event);
  }

  createTicket(ticket: NewTicket) {
    return this.http.post(`${this.baseUrl}/kontingentKarata`, ticket);
  }
}
