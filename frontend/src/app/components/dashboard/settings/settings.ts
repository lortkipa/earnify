import { Component, signal } from '@angular/core';
import { ToastService } from '../../../services/toast-service';
import { HttpClient } from '@angular/common/http';
import { userModel, userPaypalModel } from '../../../models/user-model';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-settings',
  imports: [FormsModule],
  templateUrl: './settings.html',
  styleUrl: './settings.scss',
})
export class Settings {
  hideSecret = signal<boolean>(true)

  user = signal<userModel | null>(null)
  paypalInfo = signal<userPaypalModel>({
    paypalClientId: '',
    paypalClientSecret: ''
  })

  constructor(private toastService: ToastService, private http: HttpClient, private router: Router) {
    this.http.get<userModel>('https://localhost:7067/api/Profile', { withCredentials: true }).subscribe({
      next: (user) => {
        if (user == null) {
          this.router.navigate(['/home'])
        }

        this.user.set(user)
        this.paypalInfo.set({
          paypalClientId: user.paypalClientId,
          paypalClientSecret: user.paypalClientSecret
        })
      },
      error: (err) => {
        console.error(err);
        this.router.navigate(['/home']);
      }
    })
  }

  toggleSecret(): void {
    this.hideSecret.set(!this.hideSecret())
  }

  updatePaypal(): void {
    this.http.patch<void>(`https://localhost:7067/api/Profile/Paypal/${this.user()?.id}`, this.paypalInfo(), { withCredentials: true }).subscribe({
      next: (user) => {
        this.toastService.show('paypal info updated', 'success');
      },
      error: (err) => {
        this.toastService.show('failed to update paypal info', 'error');
      }
    })
  }
}
