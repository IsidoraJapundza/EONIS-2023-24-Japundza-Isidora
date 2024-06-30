import { Routes } from "@angular/router";
import { LoginComponent } from "../components/login/login.component";
import { RegisterComponent } from "../components/register/register.component";
import { OrdersComponent } from "../components/orders/orders.component";
import { EventsComponent } from "../components/events/events.component";
import { AddEventComponent } from "../components/add-event/add-event.component";
import { DashboardComponent } from "../components/dashboard/dashboard.component";
import { PaymentsComponent } from "../components/payments/payments.component";

export const appRoutes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'events', component: EventsComponent},
  {path: 'add-event', component: AddEventComponent},
  {path: 'orders', component: OrdersComponent},
  {path: 'dashboard', component: DashboardComponent},
  {path: 'payments/:userId', component: PaymentsComponent},
]
