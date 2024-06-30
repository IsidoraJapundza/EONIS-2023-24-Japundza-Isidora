import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { appRoutes } from 'src/app/common/route-config';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { OrdersComponent } from './components/orders/orders.component';
import { AuthInterceptor } from './common/auth.interceptor';
import { EventsComponent } from './components/events/events.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AddEventComponent } from './components/add-event/add-event.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PaymentsComponent } from './components/payments/payments.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    LoginComponent,
    RegisterComponent,
    RegisterComponent,
    OrdersComponent,
    EventsComponent,
    AddEventComponent,
    DashboardComponent,
    PaymentsComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
    NgbModule,
    FormsModule
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
