import { ChangeDetectionStrategy, Component, ElementRef, HostListener, inject, OnDestroy, OnInit, Renderer2, ViewEncapsulation } from '@angular/core';

@Component({
  imports: [],
  templateUrl: './layouts.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Layouts implements OnInit, OnDestroy {
  private resizeTimer: any;

  readonly #elementRef = inject(ElementRef);
  readonly #renderer = inject(Renderer2);

  ngOnInit(): void {
    this.initializeSidebar();
    this.initializeSubmenus();
    this.setActiveMenuItem();
    this.loadSidebarState();
  }

  ngOnDestroy(): void {
    if (this.resizeTimer) {
      clearTimeout(this.resizeTimer);
    }
  }

  @HostListener('window:resize', ['$event'])
  onWindowResize(event: any): void {
    clearTimeout(this.resizeTimer);
    this.resizeTimer = setTimeout(() => {
      this.handleWindowResize();
    }, 250);
  }

  private initializeSidebar(): void {
    const sidebarToggle = this.#elementRef.nativeElement.querySelector('#sidebarToggle');
    const mobileSidebarToggle = this.#elementRef.nativeElement.querySelector('.mobile-sidebar-toggle');

    if (sidebarToggle) {
      this.#renderer.listen(sidebarToggle, 'click', () => {
        this.toggleSidebar();
      });
    }

    if (mobileSidebarToggle) {
      this.#renderer.listen(mobileSidebarToggle, 'click', () => {
        this.toggleMobileSidebar();
      });
    }
  }

  private initializeSubmenus(): void {
    const menuItems = this.#elementRef.nativeElement.querySelectorAll('.menu-item');

    menuItems.forEach((item: HTMLElement) => {
      const menuLink = item.querySelector('.menu-link');
      const submenu = item.querySelector('.submenu');

      if (submenu && menuLink) {
        this.#renderer.listen(menuLink, 'click', (e: Event) => {
          e.preventDefault();
          this.toggleSubmenu(item, menuItems);
        });
      }
    });
  }

  private setActiveMenuItem(): void {
    const currentPath = window.location.pathname;
    const menuLinks = this.#elementRef.nativeElement.querySelectorAll('.menu-link, .submenu a');

    menuLinks.forEach((link: HTMLElement) => {
      const href = link.getAttribute('href');
      if (href === currentPath) {
        const menuItem = link.closest('.menu-item');
        if (menuItem) {
          this.#renderer.addClass(menuItem, 'active');

          // Open parent menu if in submenu
          const parentSubmenu = link.closest('.submenu');
          if (parentSubmenu) {
            const parentItem = parentSubmenu.closest('.menu-item');
            if (parentItem) {
              this.#renderer.addClass(parentItem, 'open');
              const submenu = parentItem.querySelector('.submenu') as HTMLElement;
              if (submenu) {
                this.#renderer.setStyle(submenu, 'maxHeight', submenu.scrollHeight + 'px');
              }
            }
          }
        }
      }
    });
  }

  private loadSidebarState(): void {
    const savedState = localStorage.getItem('sidebarCollapsed');
    const sidebar = this.#elementRef.nativeElement.querySelector('#sidebar');
    const mainWrapper = this.#elementRef.nativeElement.querySelector('.main-wrapper');

    if (savedState === 'true' && window.innerWidth > 992 && sidebar && mainWrapper) {
      this.#renderer.addClass(sidebar, 'collapsed');
      this.#renderer.setStyle(mainWrapper, 'marginLeft', '70px');
    }
  }

  private toggleSidebar(): void {
    const sidebar = this.#elementRef.nativeElement.querySelector('#sidebar');
    const mainWrapper = this.#elementRef.nativeElement.querySelector('.main-wrapper');

    if (!sidebar || !mainWrapper) return;

    const isCollapsed = sidebar.classList.contains('collapsed');

    if (isCollapsed) {
      this.#renderer.removeClass(sidebar, 'collapsed');
      this.#renderer.setStyle(mainWrapper, 'marginLeft', '280px');
    } else {
      this.#renderer.addClass(sidebar, 'collapsed');
      this.#renderer.setStyle(mainWrapper, 'marginLeft', '70px');
    }

    // Save state to localStorage
    localStorage.setItem('sidebarCollapsed', (!isCollapsed).toString());
  }

  private toggleMobileSidebar(): void {
    const sidebar = this.#elementRef.nativeElement.querySelector('#sidebar');
    if (!sidebar) return;

    const isShowing = sidebar.classList.contains('show');
    let backdrop = this.#elementRef.nativeElement.querySelector('.sidebar-backdrop');

    if (isShowing) {
      this.#renderer.removeClass(sidebar, 'show');
      if (backdrop) {
        this.#renderer.removeChild(document.body, backdrop);
      }
    } else {
      this.#renderer.addClass(sidebar, 'show');

      if (!backdrop) {
        backdrop = this.#renderer.createElement('div');
        this.#renderer.addClass(backdrop, 'sidebar-backdrop');
        this.#renderer.appendChild(document.body, backdrop);

        // Add click listener to backdrop
        this.#renderer.listen(backdrop, 'click', () => {
          this.#renderer.removeClass(sidebar, 'show');
          this.#renderer.removeChild(document.body, backdrop);
        });
      }
    }
  }

  private toggleSubmenu(item: HTMLElement, allMenuItems: NodeListOf<Element>): void {
    const submenu = item.querySelector('.submenu') as HTMLElement;
    if (!submenu) return;

    // Close other open menus (accordion effect)
    allMenuItems.forEach((otherItem: Element) => {
      if (otherItem !== item && otherItem.classList.contains('open')) {
        this.#renderer.removeClass(otherItem, 'open');
        const otherSubmenu = otherItem.querySelector('.submenu') as HTMLElement;
        if (otherSubmenu) {
          this.#renderer.setStyle(otherSubmenu, 'maxHeight', '0');
        }
      }
    });

    // Toggle current menu
    const isOpen = item.classList.contains('open');

    if (isOpen) {
      this.#renderer.removeClass(item, 'open');
      this.#renderer.setStyle(submenu, 'maxHeight', '0');
    } else {
      this.#renderer.addClass(item, 'open');
      this.#renderer.setStyle(submenu, 'maxHeight', submenu.scrollHeight + 'px');
    }
  }

  private handleWindowResize(): void {
    const sidebar = this.#elementRef.nativeElement.querySelector('#sidebar');
    const mainWrapper = this.#elementRef.nativeElement.querySelector('.main-wrapper');

    if (!sidebar || !mainWrapper) return;

    if (window.innerWidth <= 992) {
      this.#renderer.removeClass(sidebar, 'collapsed');
      this.#renderer.setStyle(mainWrapper, 'marginLeft', '0');
    } else {
      const savedState = localStorage.getItem('sidebarCollapsed');
      if (savedState === 'true') {
        this.#renderer.addClass(sidebar, 'collapsed');
        this.#renderer.setStyle(mainWrapper, 'marginLeft', '70px');
      } else {
        this.#renderer.removeClass(sidebar, 'collapsed');
        this.#renderer.setStyle(mainWrapper, 'marginLeft', '280px');
      }
    }
  }

  // Public methods that can be called from template or other components
  public toggleSidebarPublic(): void {
    this.toggleSidebar();
  }

  public showNotification(message: string): void {
    // Notification logic can be implemented here
    console.log('Notification:', message);
  }
}
