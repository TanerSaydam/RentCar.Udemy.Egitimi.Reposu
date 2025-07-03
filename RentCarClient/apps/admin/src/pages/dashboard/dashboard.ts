import { ChangeDetectionStrategy, Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { BreadcrumbService } from '../../services/breadcrumb';

@Component({
  imports: [],
  templateUrl: './dashboard.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Dashboard implements OnInit {
  readonly #breadcrumb = inject(BreadcrumbService);

  ngOnInit(): void {
    this.#breadcrumb.setDashboard();
  }
}
