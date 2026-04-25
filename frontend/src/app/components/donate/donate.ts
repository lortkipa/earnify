import { CommonModule } from '@angular/common';
import { Component, AfterViewInit, ElementRef, ViewChild, signal } from '@angular/core';

declare var paypal: any;

@Component({
  selector: 'app-donate',
  templateUrl: './donate.html',
  styleUrls: ['./donate.scss'],
  imports: [CommonModule]
})

export class Donate implements AfterViewInit {

  amount = signal<number>(15)
  customAmount = signal<boolean>(false)

  togglecustomAmount() {
    this.customAmount.set(!this.customAmount())
  }

  @ViewChild('paypalRef', { static: true }) paypalElement!: ElementRef;

  ngAfterViewInit() {
    paypal.Buttons({
      createOrder: async () => {
        const response = await fetch("https://localhost:7067/api/Payments/CreateDonation", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            amount: 50
          })
        });

        const order = await response.json();
        return order.id;
      },

      onApprove: async (data: any) => {
        const response = await fetch("https://localhost:7067/api/Payments/CompleteDonation", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            orderID: data.orderID
          })
        })

        const details = await response.json();
        console.log(details)
      },

      onError: (err: any) => {
      }

    }).render(this.paypalElement.nativeElement);
  }
}