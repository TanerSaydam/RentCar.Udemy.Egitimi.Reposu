import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { httpResource } from '@angular/common/http';
import Blank from 'apps/admin/src/components/blank/blank';
import { ReservationModel, initialReservation } from 'apps/admin/src/models/reservation.model';
import { Result } from 'apps/admin/src/models/result.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { TrCurrencyPipe } from 'tr-currency';
import { DatePipe, NgClass } from '@angular/common';
import { NgxMaskPipe } from 'ngx-mask';

@Component({
  imports: [
    Blank,
    DatePipe,
    NgClass,
    NgxMaskPipe,
    TrCurrencyPipe
  ],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class ReservationDetail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadcrumbModel[]>([]);
  readonly result = httpResource<Result<ReservationModel>>(() => `/rent/reservations/${this.id()}`);
  readonly data = computed(() => this.result.value()?.data ?? initialReservation);
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = signal<string>("Rezervasyon Detayı");

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadcrumbModel[] = [
        {
          title: 'Rezervasyonlar',
          icon: 'bi-calendar-check',
          url: '/reservations'
        }
      ];

      if (this.data()) {
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update(prev => [...prev, {
          title: this.data().reservationNumber,
          icon: 'bi-zoom-in',
          url: `/reservations/detail/${this.id()}`,
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }

  getStatusClass(){
    switch (this.data().status) {
      case 'Bekliyor': return 'bg-warning'
      case 'Teslim Edildi': return 'bg-info'
      case 'Tamamlandı': return 'bg-success'
      case 'İptal Edildi': return 'bg-danger'
      default: return ''
    }
  }
}