import {
  trigger,
  transition,
  style,
  animate,
  state,
} from '@angular/animations';

export const SharedAnimations = {

  fadeIn: trigger('fadeIn', [
    transition(':enter', [
      style({ opacity: 0 }),
      animate('900ms ease-out', style({ opacity: 1 }))
    ])
  ]),

  slideUp: trigger('slideUp', [
    state('hidden', style({
      opacity: 0,
      transform: 'translateY(50px)'
    })),
    state('visible', style({
      opacity: 1,
      transform: 'translateY(0)'
    })),
    transition('hidden => visible', animate('700ms cubic-bezier(0.175, 0.885, 0.32, 1.275)'))
  ]),

};
