import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Stripe, loadStripe } from '@stripe/stripe-js';
import { error } from 'console';
import { Order, PayRequest } from 'src/app/models/order-models';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.css'],
})
export class PaymentsComponent implements OnInit {
  orders!: Order[];
  userId!: string;
  paymentForm!: FormGroup;
  orderToPay!: string;
  stripe: Stripe | null = null;
  card: any;
  clientSecret: string | null = null;

  constructor(
    private orderService: OrderService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.userId = params.get('userId') ?? '';
    });

    if (this.userId) {
      this.orderService.getUserOrders(this.userId).subscribe({
        next: (value) => {
          this.orders = value.filter(
            (x) =>
              x.statusPorudzbine === 'U toku' &&
              x.potvrdaPlacanja === 'Za naplatu'
          );
          if (this.orders.length) {
            this.fetchStripeData();
          }
        },
        error: (error) => {
          console.error('Error:', error);
          alert('Neuspesna dobavljanje porudzbina');
        },
      });
    }
  }

  async fetchStripeData() {
    this.stripe = await loadStripe(
      'pk_test_51K1AwBDCC8Upr6zGBGJgTROKSrHAsTlOWfiW0C1JOwgfippNLjk9ySMl6rBr394h0YZaNtpm5KBB5phA6yEfToCS00ILny4O0w'
    );

    if (!this.stripe) {
      return;
    }

    const elements = this.stripe.elements();
    this.card = elements.create('card');
    this.card.mount('#card-element');
  }

  onOrderToPayChange(event: Event): void {
    const target = event.target as HTMLSelectElement | null;
    if (target) {
      this.orderToPay = target.value;
    }
  }

  async onPay() {
    if (!this.stripe || !this.card) {
      console.error('Stripe or card element not initialized');
      return;
    }

    const { paymentMethod, error } = await this.stripe.createPaymentMethod({
      type: 'card',
      card: this.card,
    });

    if (error) {
      console.error('Error creating payment method:', error);
      return;
    }

    const paymentRequest: PayRequest = {
      price:
        this.orders.find((x) => x.id_porudzbina === this.orderToPay)
          ?.ukupnaCena ?? 0,
      orderId: this.orderToPay,
      paymentMethodId: paymentMethod.id,
    };

    this.orderService.createPayment(paymentRequest).subscribe({
      next: (value) => {
        window.confirm(
          'Uplata uspesno izvrsena, u toju je izmena statusa vase porudzbine'
        );
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        console.error(error);
        alert('Placanje neuspesno zavrseno');
      },
    });
  }
}
