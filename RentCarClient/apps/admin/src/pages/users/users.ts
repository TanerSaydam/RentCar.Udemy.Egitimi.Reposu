import { ChangeDetectionStrategy, Component, inject, signal, ViewEncapsulation } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FlexiGridModule } from 'flexi-grid';
import Grid from '../../components/grid/grid';
import { BreadcrumbModel } from '../../services/breadcrumb';
import { Common } from '../../services/common';

@Component({
  imports: [
    Grid,
    FlexiGridModule,
    RouterLink
  ],
  templateUrl: './users.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Users {
readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Kullanıcılar',
      icon: 'bi-people',
      url: '/users',
      isActive: true
    }
  ]);

  readonly #common = inject(Common);

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }
}
