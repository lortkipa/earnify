import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface PaymentLink {
  id: number;
  type: 'donation' | 'product';
  name: string;
  msg?: string;
  price?: number;
  desc?: string;
  image?: string | null;
}

@Component({
  selector: 'app-links',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './links.html',
  styleUrls: ['./links.scss']
})
export class Links {
  // State using Signals
  links = signal<PaymentLink[]>(this.loadLinks());
  isModalOpen = signal(false);
  editingId = signal<number | null>(null);
  tempImg = signal<string | null>(null);

  // Form Model
  formData = {
    type: 'donation' as 'donation' | 'product',
    name: '',
    msg: '',
    price: 0,
    desc: ''
  };

  private loadLinks(): PaymentLink[] {
    const saved = localStorage.getItem('earnify_links');
    return saved ? JSON.parse(saved) : [];
  }

  toggleModal(editId: number | null = null) {
    this.editingId.set(editId);
    if (editId) {
      const link = this.links().find(l => l.id === editId);
      if (link) {
        this.formData = { ...link, type: link.type as 'donation' | 'product', msg: link.msg || '', price: link.price || 0, desc: link.desc || '' };
        this.tempImg.set(link.image || null);
      }
    } else {
      this.resetForm();
    }
    this.isModalOpen.set(!this.isModalOpen());
  }

  handleImageUpload(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => this.tempImg.set(e.target?.result as string);
      reader.readAsDataURL(file);
    }
  }

  saveLink() {
    if (!this.formData.name) return;

    const newLink: PaymentLink = {
      id: this.editingId() || Date.now(),
      ...this.formData,
      image: this.tempImg()
    };

    if (this.editingId()) {
      this.links.update(prev => prev.map(l => l.id === this.editingId() ? newLink : l));
    } else {
      this.links.update(prev => [...prev, newLink]);
    }

    localStorage.setItem('earnify_links', JSON.stringify(this.links()));
    this.toggleModal();
  }

  deleteLink(id: number) {
    this.links.update(prev => prev.filter(l => l.id !== id));
    localStorage.setItem('earnify_links', JSON.stringify(this.links()));
  }

  private resetForm() {
    this.formData = { type: 'donation', name: '', msg: '', price: 0, desc: '' };
    this.tempImg.set(null);
  }
}