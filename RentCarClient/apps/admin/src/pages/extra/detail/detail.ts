import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { httpResource } from '@angular/common/http';
import Blank from 'apps/admin/src/components/blank/blank';
import { ExtraModel, initialExtraModel } from 'apps/admin/src/models/extra.model';
import { Result } from 'apps/admin/src/models/result.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { TrCurrencyPipe } from 'tr-currency';

@Component({
  imports: [
    Blank,
    TrCurrencyPipe
  ],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class ExtraDetail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadcrumbModel[]>([]);
  readonly result = httpResource<Result<ExtraModel>>(() => `/rent/extras/${this.id()}`);
  readonly data = computed(() => this.result.value()?.data ?? initialExtraModel);
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = signal<string>("Ekstra Detay");

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadcrumbModel[] = [
        {
          title: 'Ekstralar',
          icon: 'bi-plus-square',
          url: '/extra'
        }
      ];

      if (this.data()) {
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update(prev => [...prev, {
          title: this.data().name,
          icon: 'bi-zoom-in',
          url: `/extra/detail/${this.id()}`,
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }
}