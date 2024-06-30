import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { error } from 'console';
import { AdminCreationModel, RegistrationFormModel, UserCreationModel } from 'src/app/models/user-models';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registrationForm!: FormGroup;
  registrationFormData!: RegistrationFormModel;

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    localStorage.clear();

    this.createRegistrationForm();
  }

  createRegistrationForm() {
    this.registrationForm = this.fb.group({
      name: [null, Validators.required],
      lastName: [null, Validators.required],
      userName: [null, Validators.required],
      password: [null, [Validators.required, Validators.minLength(8)]],
      email: [null, [Validators.required, Validators.email]],
      phone: [null, Validators.required],
      address: [null],
      homeTown: [null],
      postalCode: [null],
      dateOfBirth: [null]
    });
  }

  onRegister(isAdmin: boolean) {
    if(isAdmin) {
      this.userService.registerAdmin(this.createAdminPayload()).subscribe({
        next: value => {
          window.confirm("Admin je uspesno registrovan")
          this.router.navigate([''])
        }, error: error => {
          console.error(error);
          alert("Neuspesna registracija admina");
          this.router.navigate(['register']);
        }
      })
    } else {
      if(!this.registrationForm.value.address || !this.registrationForm.value.postalCode || !this.registrationForm.value.homeTown || !this.registrationForm.value.dateOfBirth || `${this.registrationForm.value.postalCode}`.length !== 5) {
        alert("Neispravno popunjena forma za registraciju korisnika");
        return;
      }
      this.userService.registerUser(this.createUserPayload()).subscribe({
        next: value => {
          window.confirm("Korisnik je uspesno registrovan")
          this.router.navigate([''])
        }, error: error => {
          console.error(error);
          alert("Neuspesna registracija korisnika");
          this.router.navigate(['register']);
        }
      })
    }
  }

  createAdminPayload(): AdminCreationModel {
    return {
      ImeAdministratora: this.registrationForm.value.name,
      KontaktAdministratora: this.registrationForm.value.phone,
      KorisnickoImeAdministratora: this.registrationForm.value.userName,
      MejlAdministratora: this.registrationForm.value.email,
      PrezimeAdministratora: this.registrationForm.value.lastName,
      LozinkaAdministratora: this.registrationForm.value.password,
      Privilegije: 'Upravljanje kartama',
      StatusAktivnosti: 'Aktivan'
    }
  }

  createUserPayload(): UserCreationModel {
    return {
      ImeKorisnika: this.registrationForm.value.name,
      KontaktKorisnika: this.registrationForm.value.phone,
      KorisnickoImeKorisnika: this.registrationForm.value.userName,
      MejlKorisnika: this.registrationForm.value.email,
      PrezimeKorisnika: this.registrationForm.value.lastName,
      LozinkaKorisnika: this.registrationForm.value.password,
      AdresaKorisnika: this.registrationForm.value.address,
      PostanskiBroj: this.registrationForm.value.postalCode,
      PrebivalisteKorisnika: this.registrationForm.value.homeTown,
      DatumRodjenjaKorisnika: this.registrationForm.value.dateOfBirth
    }
  }
}
