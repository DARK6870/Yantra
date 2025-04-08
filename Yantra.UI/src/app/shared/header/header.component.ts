import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

}

document.addEventListener('DOMContentLoaded', () => {
  const menuIcon = document.querySelector(".menu-icon") as HTMLElement;
  const navUl = document.querySelector("nav ul") as HTMLUListElement;
  const nav = document.querySelector("nav") as HTMLElement;

  if (menuIcon) {
    menuIcon.addEventListener("click", (e) => {
      e.stopPropagation();
      navUl.classList.toggle("showing");
    });
  }

  // Закрывать меню при клике вне его
  document.addEventListener('click', (e) => {
    if (!(e.target as Element).closest('nav ul') && !(e.target as Element).closest('.menu-icon')) {
      navUl.classList.remove("showing");
    }
  });

  window.addEventListener("scroll", () => {
    if (nav) {
      if (window.scrollY > 0) {
        nav.classList.add('black');
      } else {
        nav.classList.remove('black');
      }
    }
  });
});
