import { Component, signal } from '@angular/core';
import { userModel } from '../../../models/user-model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-overview',
  imports: [CommonModule],
  templateUrl: './overview.html',
  styleUrl: './overview.scss',
})
export class Overview {
  user = signal<userModel | null>(null)

  constructor(private http: HttpClient, private router: Router) {
    this.http.get<userModel>('https://localhost:7067/api/Profile', { withCredentials: true }).subscribe({
      next: (user) => {
        if (user == null) {
          this.router.navigate(['/home'])
        }

        this.user.set(user)
      },
      error: (err) => {
        console.error(err);
        this.router.navigate(['/home']);
      }
    })
  }
}
