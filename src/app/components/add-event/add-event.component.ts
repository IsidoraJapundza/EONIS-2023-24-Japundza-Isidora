import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Event, NewEvent, NewTicket, Ticket } from 'src/app/models/event-models';
import { AdminModel } from 'src/app/models/user-models';
import { EventService } from 'src/app/services/event.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-event',
  templateUrl: './add-event.component.html',
  styleUrls: ['./add-event.component.css']
})
export class AddEventComponent implements OnInit {

  addEventForm!: FormGroup;
  event!: NewEvent;
  ticket!: NewTicket;
  usesrName!: string;
  admin!: AdminModel;

  constructor(private fb: FormBuilder, private eventService: EventService, private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    const localStorageData = localStorage.getItem("user-name");
    if(localStorageData) {
      this.usesrName = localStorageData;
      this.createAddEventForm();
      this.userService.getAdmin(this.usesrName).subscribe({
        next: value => {
          this.admin = value;
        },
        error: error => {
          console.error('Error:', error);
          alert("Neuspesno dobavljanje administratora");
        }
      })
      return;
    }

    this.router.navigate(['/'])
  }

  createAddEventForm() {
    this.addEventForm = this.fb.group({
      eventName: [null, Validators.required],
      eventDate: [null, Validators.required],
      eventStart: [null, Validators.required],
      eventEnd: [null, Validators.required],
      placeOfEvent: [null, Validators.required],
      ticketName: [null, Validators.required],
      sector: [null, Validators.required],
      entrance: [null, Validators.required],
      price: [null, [Validators.required, Validators.pattern("^[0-9]*$")]],
      amount: [null, [Validators.required, Validators.pattern("^[0-9]*$")]],
      note: [''],
    })
  }

  onSubmit() {
    if(!this.addEventForm.valid) {
      console.error('Nevalidna forma - pravilo popunite polja');
    } else if (this.addEventForm.value.eventStart >= this.addEventForm.value.eventEnd) {
      console.error('Nevalidna forma - nepravilno popunjena polja za vreme dogadjaja');
    } else {
      this.eventService.createEvent(this.createEventPayload()).subscribe({
        next: value => {
          if(value.id_dogadjaj) {
            this.eventService.createTicket(this.createTicketPayload(value.id_dogadjaj)).subscribe({
              next: value => {
                window.confirm("Uspesno kreiran dogadjaj kao i kontingent karata");
                this.router.navigate(['events'])
              }
            })
          }
        }
      })
    }
  }

  createEventPayload(): NewEvent {
    return {
      nazivSportskogDogadjaja: this.addEventForm.value.eventName,
      datumOdrzavanja: this.addEventForm.value.eventDate,
      vremeOdrzavanja: this.addEventForm.value.eventStart,
      predvidjenoVremeZavrsetka: this.addEventForm.value.eventEnd,
      mestoOdrzavanja: this.addEventForm.value.placeOfEvent
    }
  }

  createTicketPayload(eventId: string): NewTicket {
    return {
      nazivKarte: this.addEventForm.value.ticketName,
      sektor: this.addEventForm.value.sector,
      ulaz: this.addEventForm.value.entrance,
      cena: this.addEventForm.value.price,
      kolicina: this.addEventForm.value.amount,
      napomena: this.addEventForm.value.note,
      id_administrator: this.admin.id_administrator,
      id_dogadjaj: eventId,
    }
  }
}
