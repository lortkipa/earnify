import { Injectable, signal } from '@angular/core';
import { donationLink } from '../models/link-model';
import { HttpClient } from '@angular/common/http';
import { catchError, of } from 'rxjs';
import { ToastService } from './toast-service';

@Injectable({
  providedIn: 'root',
})
export class DonationLinkService {
  data = signal<donationLink[]>([])

  constructor(private http: HttpClient, private toastService: ToastService) {
    this.refreshData()
  }

  refreshData() {
    this.http.get<donationLink[]>('https://localhost:7067/api/DonationLink', { withCredentials: true }).pipe(
      catchError(() => {
        return of(null);
      })
    ).subscribe(data => {
      this.data.set(data ?? [])
    });
  }

  create(msg: string) {
    const body = JSON.stringify(msg);

    this.http.post<donationLink>(
      `https://localhost:7067/api/DonationLink`,
      body,
      {
        withCredentials: true,
        headers: { 'Content-Type': 'application/json' }
      }
    ).subscribe({
      next: (newData) => {
        this.data.update(currentItems => [...currentItems, newData]);
        this.toastService.show('New Donation Link Created', 'success')
      },
      error: () => { this.toastService.show('Failed to Create Donation Link', 'error') }
    });
  }

  delete(id: number) {
    this.http.delete<void>(`https://localhost:7067/api/DonationLink/${id}`, { withCredentials: true }).subscribe({
      next: () => {
        this.data.update(currentItems =>
          currentItems.filter(item => item.id !== id)
        );
        this.toastService.show('Donation Link Deleted', 'success')
      },
      error: () => { this.toastService.show('Failed to Delete Donation Link', 'error') }
    })
  }
}
