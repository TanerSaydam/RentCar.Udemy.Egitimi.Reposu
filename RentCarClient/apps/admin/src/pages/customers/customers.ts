import { ChangeDetectionStrategy, Component, signal, ViewEncapsulation } from '@angular/core';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { inject } from '@angular/core';
import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';

@Component({
    imports: [
        Grid,
        FlexiGridModule
    ],
  templateUrl: './customers.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class CustomersPage {
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Müşteriler',
      icon: 'bi-people',
      url: '/customers',
      isActive: true
    }
  ]);

  constructor() {
    inject(BreadcrumbService).reset(this.bredcrumbs());
  }
}