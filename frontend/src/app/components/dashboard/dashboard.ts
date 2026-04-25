import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { Router, RouterOutlet, RouterLinkWithHref, RouterLink, RouterLinkActive } from '@angular/router';
import { userModel } from '../../models/user-model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard {
  user = signal<userModel | null>(null)

  isSidebarOpen = false

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

  logout() {
    this.http.get('https://localhost:7067/api/Profile/Logout', { withCredentials: true }).subscribe({
      next: (user) => { this.router.navigate(['/home']) },
      error: (err) => { this.router.navigate(['/home']) }
    })
  }
}
