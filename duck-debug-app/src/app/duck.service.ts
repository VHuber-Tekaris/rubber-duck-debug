import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Consultation, Duck } from './duck';

/**
 * All URLs are relative on purpose. The browser only ever talks to the port the
 * frontend is served on - nginx forwards everything under /api to the backend
 * container. There is no api host to configure here, and that is the point.
 */
@Injectable({ providedIn: 'root' })
export class DuckService {
  private readonly http = inject(HttpClient);

  getDucks() {
    return this.http.get<Duck[]>('/api/ducks');
  }

  getConsultations() {
    return this.http.get<Consultation[]>('/api/consultations');
  }

  consult(duckId: number, problem: string) {
    return this.http.post<Consultation>('/api/consultations', { duckId, problem });
  }
}
