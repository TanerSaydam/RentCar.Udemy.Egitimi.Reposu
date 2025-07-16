import { ChangeDetectionStrategy, Component, computed, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { BreadcrumbService } from '../../services/breadcrumb';
import Blank from '../../components/blank/blank';
import { httpResource } from '@angular/common/http';
import { Result } from '../../models/result.model';
import Loading from '../../components/loading/loading';

@Component({
  imports: [
    Blank,
    Loading
  ],
  templateUrl: './dashboard.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Dashboard implements OnInit {
  readonly activeReservationCountResult = httpResource<Result<number>>(() => "/rent/dashboard/active-reservation-count");
  readonly activeReservationCount = computed(() => this.activeReservationCountResult.value()?.data ?? 0);
  readonly activeReservationCountLoading = computed(() => this.activeReservationCountResult.isLoading());

  readonly #breadcrumb = inject(BreadcrumbService);

  ngOnInit(): void {
    this.#breadcrumb.setDashboard();
  }
}
