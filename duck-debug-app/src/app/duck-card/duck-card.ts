import { Component, input, output } from '@angular/core';
import { Duck } from '../duck';

@Component({
  selector: 'app-duck-card',
  templateUrl: './duck-card.html',
  styleUrl: './duck-card.css'
})
export class DuckCard {
  readonly duck = input.required<Duck>();
  readonly selected = input(false);
  readonly pick = output<Duck>();
}
