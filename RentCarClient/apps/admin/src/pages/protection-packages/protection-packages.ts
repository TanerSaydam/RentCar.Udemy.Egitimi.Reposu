import { ChangeDetectionStrategy, Component, ElementRef, inject, signal, viewChild, ViewEncapsulation } from '@angular/core';
import { FlexiGridModule, FlexiGridReorderModel } from 'flexi-grid';
import Grid from '../../components/grid/grid';
import { BreadcrumbModel } from '../../services/breadcrumb';
import { Common } from '../../services/common';
import { ProtectionPackageModel } from '../../models/protection-package.model';
import { HttpService } from '../../services/http';

@Component({
  imports: [
    Grid,
    FlexiGridModule,
  ],
  templateUrl: './protection-packages.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class ProtectionPackages {
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Güvence Paketleri',
      icon: 'bi-shield-check',
      url: '/protection-packages',
      isActive: true
    }
  ]);

  readonly grid = viewChild<any>("grid");

  readonly #common = inject(Common);
  readonly #http = inject(HttpService);

  checkPermission(permission: string){
    return this.#common.checkPermission(permission);
  }

  onReorder(event:FlexiGridReorderModel){
    const data:ProtectionPackageModel = event.previousData;
    data.orderNumber = event.currentData.orderNumber;

    this.#http.put('/rent/protection-packages', data,()=> {
      this.grid()!.result.reload();
    })
  }
}