import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, of } from 'rxjs';
import { userModel, userPaypalModel } from '../models/user-model';
import { ToastService } from './toast-service';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  data = signal<userModel | null>(null);

  constructor(private http: HttpClient, private router: Router, private toastService: ToastService) {
    this.refreshData()
  }

  refreshData() {
    this.http.get<userModel>('https://localhost:7067/api/Profile', { withCredentials: true }).pipe(
      catchError(() => {
        return of(null);
      })
    ).subscribe(user => {
      this.data.set(user);
      const currentRoute = this.router.url;

      if (user) {
        if (currentRoute === '/' || currentRoute === '/home') {
          this.router.navigate(['/dashboard']);
        }
      } else {
        if (currentRoute.includes('/dashboard')) {
          this.router.navigate(['/home']);
        }
      }
    });
  }

  logout() {
    this.http.get('https://localhost:7067/api/Profile/Logout', { withCredentials: true }).subscribe({
      next: (user) => this.router.navigate(['/home']),
      error: (err) => this.router.navigate(['/home'])
    })
  }

  updatePaypal(data: userPaypalModel): void {
    this.http.patch<void>(`https://localhost:7067/api/Profile/Paypal/${this.data()?.id}`, data, { withCredentials: true }).subscribe({
      next: () => {
        this.toastService.show('paypal info updated', 'success');

        this.data.update(currentData => {
          if (currentData) {
            return {
              ...currentData,
              paypalClientId: data.paypalClientId,
              paypalClientSecret: data.paypalClientSecret  
            };
          }
          
          return null;
        });
      },
      error: () => {
        this.toastService.show('failed to update paypal info', 'error');
      }
    })
  }
}