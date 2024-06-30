import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { Event, Ticket } from 'src/app/models/event-models';
import { EventService } from 'src/app/services/event.service';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {

  events!: Event[];
  tickets!: Ticket[];

  constructor(private eventService: EventService) { }

  ngOnInit(): void {
    forkJoin({
      events: this.eventService.getAllEvents(),
      tickets: this.eventService.getAllTickets()
    }).subscribe({
      next: results => {
        this.events = results.events;
        this.tickets = results.tickets;
      },
      error: error => {
        console.error('Error:', error);
        alert("Neuspesna dobavljanje podataka");
      }
    });
  }

  getNazivKarte(id_dogadjaj: string): string | undefined {
    return this.tickets.find(x => x.id_dogadjaj === id_dogadjaj)?.nazivKarte;
  }

  getSektor(id_dogadjaj: string): string | undefined {
    return this.tickets.find(x => x.id_dogadjaj === id_dogadjaj)?.sektor;
  }

  getUlaz(id_dogadjaj: string): string | undefined {
    return this.tickets.find(x => x.id_dogadjaj === id_dogadjaj)?.ulaz;
  }

  getBrKarata(id_dogadjaj: string): string | undefined {
    return this.tickets.find(x => x.id_dogadjaj === id_dogadjaj)?.kolicina + '';
  }
}
