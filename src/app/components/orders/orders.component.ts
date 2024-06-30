import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/models/order-models';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  orders!: Order[];

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
    this.orderService.getAllOrders().subscribe({
      next: value => {
        this.orders = value
      },
      error: error => {
        console.error('Error:', error);
        alert("Neuspesna dobavljanje porudzbina")
      }
    })
  }

  onRemoveOrder(order: Order): void {
    if(order.id_korisnik && order.id_porudzbina && order.id_kontingentKarata) {
      this.orderService.deleteOrder(order.id_porudzbina, order.id_korisnik, order.id_kontingentKarata).subscribe({
        next: value => {
          alert("Uspesno izbrisana porudzbina")
          window.location.reload()
        },
        error: error => {
          console.error('Error:', error);
          alert("Neuspesna dobavljanje porudzbina")
        }
      })
    } else {
      alert("Neuspesno brisanje porudzbine");
    }
  }

}
