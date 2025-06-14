import { Component } from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedAnimations} from '../../shared/shared-animations';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  animations: [
    SharedAnimations.fadeIn,
    SharedAnimations.slideUp
  ]
})

export class HomeComponent {
  titleVariations = [
    "A taste of home, a touch of poetry",
    "In every corner, a comfort; in every meal, a hug",
    "Warm meals. Warmer memories",
    "Mangia bene, ridi spesso, ama molto",
    "Not just food. A feeling"
  ]

  currentTitle: string;
  animationState = {
    tradition: 'hidden',
    awards: 'hidden'
  };
  constructor() {
    this.currentTitle = this.getRandomTitle(this.titleVariations);
  }
  private getRandomTitle(array: string[]) : string {
    return array[Math.floor(Math.random() * array.length)];
  }
}
