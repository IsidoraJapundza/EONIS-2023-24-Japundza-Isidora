import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { error } from 'console';
import { RoleClaim, UserNameClaim } from 'src/app/common/constants';
import { decodeToken } from 'src/app/common/functions';
import { AuthCreds } from 'src/app/models/user-models';
import { UserService } from 'src/app/services/user.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  authCreds!: AuthCreds;
  loginForm!: FormGroup;

  constructor(private userService: UserService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    localStorage.clear();

    this.loginForm = this.fb.group({
      korisnickoIme: [null, Validators.required],
      lozinka: [null, Validators.required]
    })
  }

  createLoginForm() {
    this.loginForm = this.fb.group({
      korisnickoIme: [null, Validators.required],
      lozinka: [null, Validators.required]
    })
  }

  onLogin(isAdmin: boolean) {
    if(this.loginForm.valid) {
      this.userService.login(this.authData(), isAdmin).subscribe({
        next: value => {
          const token = decodeToken(`${value.token}`);

          if(!(token?.[RoleClaim] && token[UserNameClaim])) {
            return;
          }
          localStorage.setItem('token', value.token)
          localStorage.setItem('role', token?.[RoleClaim])
          localStorage.setItem('user-name', token?.[UserNameClaim])

          this.router.navigate([isAdmin ? '/orders' : '/dashboard'])
        },
        error: error => {
          console.error('Error:', error);
          alert("Neuspesna prijava")
        }
      })
    }
  }

  authData(): AuthCreds {
    return this.authCreds = {
      korisnickoIme: this.email.value,
      lozinka: this.password.value
    }
  }

  get email() {
    return this.loginForm.get('korisnickoIme') as FormControl;
  }
  get password() {
    return this.loginForm.get('lozinka') as FormControl;
  }
}
