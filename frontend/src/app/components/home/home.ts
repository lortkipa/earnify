import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthConfig } from 'angular-oauth2-oidc';
import { catchError, of } from 'rxjs';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})

export class Home {
  constructor(private http: HttpClient, private router: Router) {
    this.http.get('https://localhost:7067/api/Profile', { withCredentials: true }).pipe(
      catchError(() => {
        return of(null);
      })
    ).subscribe(user => {
      if (user) {
        this.router.navigate(['/dashboard']);
      }
    });
  }

  googleAuth() {
    window.location.href = 'https://localhost:7067/api/Auth/Google'
  }
}
