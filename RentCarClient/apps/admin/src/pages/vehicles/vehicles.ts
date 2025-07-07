import { ChangeDetectionStrategy, Component, computed, inject, signal, ViewEncapsulation } from '@angular/core';
import { FlexiGridFilterDataModel, FlexiGridModule } from 'flexi-grid';
import Grid from '../../components/grid/grid';
import { BreadcrumbModel } from '../../services/breadcrumb';
import { Common } from '../../services/common';
import { brandList, colorList, modelYearList } from './create/create';

@Component({
  imports: [
    Grid,
    FlexiGridModule,
  ],
  templateUrl: './vehicles.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Vehicles {
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Araçlar',
      icon: 'bi-car-front',
      url: '/vehicles',
      isActive: true
    }
  ]);
  readonly brandFilterData = computed<FlexiGridFilterDataModel[]>(() => 
    brandList.map(val => ({
        value: val,
        name: val
      }))
  );
  readonly modelYearFilterData = computed<FlexiGridFilterDataModel[]>(() => 
    modelYearList.map(val => ({
        value: val,
        name: val
      }))
  );
  readonly colorFilterData = computed<FlexiGridFilterDataModel[]>(() => 
    colorList.map(val => ({
        value: val,
        name: val
      }))
  );

  readonly #common = inject(Common);

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }
}