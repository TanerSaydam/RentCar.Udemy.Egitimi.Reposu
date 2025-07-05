import { ChangeDetectionStrategy, Component, signal, ViewEncapsulation } from '@angular/core';
import { BreadcrumbModel } from '../../services/breadcrumb';
import { FlexiGridModule } from 'flexi-grid';
import Grid from '../../components/grid/grid';
import { RouterLink } from '@angular/router';

@Component({
  imports: [
    Grid,
    FlexiGridModule,
    RouterLink
  ],
  templateUrl: './roles.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Roles {
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Roller',
      icon: 'bi-clipboard2-check',
      url: '/roles',
      isActive: true
    }
  ]);
}
