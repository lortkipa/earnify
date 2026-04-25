import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard {
  constructor(private http: HttpClient, private router: Router) {
    this.http.get('https://localhost:7067/api/Profile', { withCredentials: true }).subscribe({
      next: (user) => {
        if (user == null) {
          this.router.navigate(['/home'])
        }
      },
      error: (err) => { console.error(err) }
    })
  }

  logout() {
    this.http.get('https://localhost:7067/api/Profile/Logout', { withCredentials: true }).subscribe({
      next: (user) => { this.router.navigate(['/home']) },
      error: (err) => { this.router.navigate(['/home']) }
    })
  }
}
