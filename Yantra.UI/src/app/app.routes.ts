import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import {MenuComponent} from './pages/menu/menu.component';
import {HelpCenterComponent} from './pages/help-center/help-center.component';
import {AboutComponent} from './pages/about/about.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    title: 'Yantra - Home'
  },
  {
    path: 'menu',
    component: MenuComponent,
    title: 'Yantra - Menu'
  },
  {
    path: 'help-center',
    component: HelpCenterComponent,
    title: 'Yantra - Help Center'
  },
  {
    path: 'about',
    component: AboutComponent,
    title: 'Yantra - About'
  }
];
