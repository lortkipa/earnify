import { Component, effect, signal } from '@angular/core';
import { userPaypalModel } from '../../../models/user-model';
import { FormsModule } from '@angular/forms';
import { ProfileService } from '../../../services/profile-service';

@Component({
  standalone: true,
  selector: 'app-settings',
  imports: [FormsModule],
  templateUrl: './settings.html',
  styleUrl: './settings.scss',
})
export class Settings {
  hideSecret = signal<boolean>(true)

  paypalInfo = signal<userPaypalModel>({
    paypalClientId: '',
    paypalClientSecret: ''
  })

  constructor(public profileService: ProfileService) {
    effect(() => {
      const userData = this.profileService.data();

      if (userData) {
        this.paypalInfo.set({
          paypalClientId: userData.paypalClientId ?? '',
          paypalClientSecret: userData.paypalClientSecret ?? ''
        });
      }
    });
  }

  toggleSecret(): void {
    this.hideSecret.set(!this.hideSecret())
  }

  updatePaypal(): void {
    this.profileService.updatePaypal(this.paypalInfo())
  }
}
