import { Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DuckCard } from './duck-card/duck-card';
import { Consultation, Duck } from './duck';
import { DuckService } from './duck.service';

@Component({
  selector: 'app-root',
  imports: [DatePipe, FormsModule, DuckCard],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private readonly ducks = inject(DuckService);

  protected readonly roster = signal<Duck[]>([]);
  protected readonly picked = signal<Duck | null>(null);
  protected readonly problem = signal('');
  protected readonly latest = signal<Consultation | null>(null);
  protected readonly log = signal<Consultation[]>([]);
  protected readonly error = signal<string | null>(null);
  protected readonly busy = signal(false);

  constructor() {
    this.load();
  }

  protected pick(duck: Duck) {
    this.picked.set(duck);
    this.latest.set(null);
  }

  protected ask() {
    const duck = this.picked();
    const problem = this.problem().trim();
    if (!duck || !problem || this.busy()) {
      return;
    }

    this.busy.set(true);
    this.ducks.consult(duck.id, problem).subscribe({
      next: (consultation) => {
        this.latest.set(consultation);
        this.log.update((entries) => [consultation, ...entries]);
        this.problem.set('');
        this.error.set(null);
        this.busy.set(false);
      },
      error: () => {
        this.error.set('The duck did not answer. Is the api reachable?');
        this.busy.set(false);
      }
    });
  }

  private load() {
    this.ducks.getDucks().subscribe({
      next: (roster) => {
        this.roster.set(roster);
        this.error.set(null);
      },
      error: () => this.error.set('Could not load the ducks. Is the api reachable?')
    });

    this.ducks.getConsultations().subscribe({
      next: (entries) => this.log.set(entries),
      error: () => {
        /* The banner from getDucks is enough - no need to say it twice. */
      }
    });
  }
}
