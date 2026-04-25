import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ToastData {
  message: string;
  type: 'success' | 'error';
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private toastSource = new BehaviorSubject<ToastData | null>(null);

  toast$ = this.toastSource.asObservable();

  show(message: string, type: 'success' | 'error' = 'success') {
    this.toastSource.next({ message, type });

    setTimeout(() => {
      this.hide();
    }, 3000);
  }

  hide() {
    this.toastSource.next(null);
  }
}