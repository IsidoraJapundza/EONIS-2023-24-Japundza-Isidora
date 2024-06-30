import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Ticket, Event as EventModal } from 'src/app/models/event-models';
import { NewOrder } from 'src/app/models/order-models';
import { AdminModel, UserModel } from 'src/app/models/user-models';
import { EventService } from 'src/app/services/event.service';
import { OrderService } from 'src/app/services/order.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  events!: EventModal[];
  tickets!: Ticket[];
  filteredEvents: EventModal[] = [];
  searchTerm: string = '';
  paging: number = 1;
  pageSize: number = 5;
  newOrder!: NewOrder;
  usesrName!: string;
  user!: UserModel;

  constructor(private eventService: EventService, private orderService: OrderService, private userService: UserService, private router: Router) { }


  ngOnInit(): void {
    const localStorageData = localStorage.getItem("user-name");
    if(localStorageData) {
      this.usesrName = localStorageData;
      this.userService.getUser(this.usesrName).subscribe({
        next: value => {
          this.user = value;
          this.fetchData(this.pageSize, this.paging);
        },
        error: error => {
          console.error('Error:', error);
          alert("Neuspesno dobavljanje administratora");
          this.router.navigate(['/'])
        }
      })
    }
  }

  fetchData(pageSize: number, pageNumber: number): void {
    forkJoin({
      events: this.eventService.getAllEvents(pageSize, pageNumber),
      tickets: this.eventService.getAllTickets()
    }).subscribe({
      next: results => {
        this.events = results.events;
        this.tickets = results.tickets;
        this.filteredEvents = [...this.events];
      },
      error: error => {
        if(this.paging > 1) {
          this.paging--;
        }
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

  onSortChange(event: Event): void {
    const target = event.target as HTMLSelectElement | null;
    if (target) {
      const sortBy = target.value;
      switch (sortBy) {
        case 'naziv':
          this.sortEventsByName();
          break;
        case 'datum':
          this.sortEventsByDate();
          break;
        case 'mesto':
          this.sortEventsByLocation();
          break;
        default:
          break;
      }

      this.filterEvents();
    }
  }

  sortEventsByName(): void {
    this.events.sort((a, b) => a.nazivSportskogDogadjaja.localeCompare(b.nazivSportskogDogadjaja));
  }

  sortEventsByDate(): void {
    this.events.sort((a, b) => new Date(a.datumOdrzavanja).getTime() - new Date(b.datumOdrzavanja).getTime());
  }

  sortEventsByLocation(): void {
    this.events.sort((a, b) => a.mestoOdrzavanja.localeCompare(b.mestoOdrzavanja));
  }

  filterEvents(): void {
    if(this.events.length) {
      const searchTermLower = this.searchTerm.toLowerCase();
      this.filteredEvents = this.events.filter(event =>
        event.nazivSportskogDogadjaja.toLowerCase().includes(searchTermLower) ||
        event.datumOdrzavanja.toLowerCase().includes(searchTermLower) ||
        event.mestoOdrzavanja.toLowerCase().includes(searchTermLower)
      );
    }
  }

  onChangePage(isNextPage: boolean): void {
    this.paging = isNextPage ? (this.paging + 1) : (this.paging - 1);
    this.fetchData(this.pageSize, this.paging)
  }

  onOrderTicket(eventId: string): void {
    const ticketId = this.tickets.find(x => x.id_dogadjaj === eventId);

    if(ticketId) {
      this.newOrder = {
        brojKarata: 1,
        statusPorudzbine: 'U toku',
        metodaIsporuke: 'Pdf mejl',
        potvrdaPlacanja: 'Za naplatu',
        adresaIsporuke: this.user.adresaKorisnika,
        dodatneNapomene: '',
        id_korisnik: this.user.id_korisnik,
        id_kontingentKarata: ticketId.id_kontingentKarata
      }

      this.orderService.createOrder(this.newOrder).subscribe({
        next: value => {
          window.confirm(`Porudzbina ${value.id_porudzbina} je uspesno kreirana.`)
          this.router.navigate([`payments/${this.newOrder.id_korisnik}`])
        },
        error: error => {
          console.error('Error:', error);
          alert("Neuspesno kreiranje porudzbine");
        }
      })
    } else {
      alert("Neuspesno kreiranje porudzbine - problem s kartama");
    }
  }
}


