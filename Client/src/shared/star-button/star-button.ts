import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-star-button',
  imports: [],
  templateUrl: './star-button.html',
  styleUrl: './star-button.css'
})
export class StarButton {
  disabled = input(false);
  selected = input(false);
  clickEvent = output<Event>();

  onEvent(event: Event) {
    this.clickEvent.emit(event);
  }
}
