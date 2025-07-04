import { ChangeDetectionStrategy, Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { BreadcrumbService } from '../../services/breadcrumb';
import Blank from '../../components/blank/blank';
import { httpResource } from '@angular/common/http';

@Component({
  imports: [
    Blank
  ],
  templateUrl: './dashboard.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Dashboard implements OnInit {
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly result = httpResource(() => "/rent/");

  ngOnInit(): void {
    this.#breadcrumb.setDashboard();
  }

  makeRequest(){
    this.result.reload();
  }
}
