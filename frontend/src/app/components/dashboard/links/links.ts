import { Component, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProfileService } from '../../../services/profile-service';
import { DonationLinkService } from '../../../services/donation-link-service';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-links',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './links.html',
  styleUrls: ['./links.scss']
})
export class Links {
  showForm = signal<boolean>(false);

  donationLinkMessage = signal<string>('')

  constructor(public profileService: ProfileService, public donationLinkService: DonationLinkService) {}

  toggleShowForm() {
    this.showForm.set(!this.showForm());
  }

  createLink() {
    this.donationLinkService.create(this.donationLinkMessage());
    this.toggleShowForm()
  }

  deleteLink(id: number) {
    this.donationLinkService.delete(id)
  }
}