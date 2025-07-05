import { ChangeDetectionStrategy, Component, computed, inject, signal, ViewEncapsulation } from '@angular/core';
import { BreadcrumbModel, BreadcrumbService } from '../../services/breadcrumb';
import { httpResource } from '@angular/common/http';
import { ODataModel } from '../../models/odata.model';
import { BranchModel } from '../../models/branch.model';
import { FlexiGridModule, FlexiGridService, StateModel } from 'flexi-grid';
import { NgxMaskPipe } from 'ngx-mask';
import { RouterLink } from '@angular/router';

@Component({
  imports: [
    FlexiGridModule,
    NgxMaskPipe,
    RouterLink
  ],
  templateUrl: './branches.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Branches {
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Şubeler',
      icon: 'bi-buildings',
      url: '/branches',
      isActive: true
    }
  ]);
  readonly state = signal<StateModel>(new StateModel());
  readonly result = httpResource<ODataModel<BranchModel>>(() => {
    let endpoint = '/rent/odata/branches?$count=true';
    let part = this.#grid.getODataEndpoint(this.state());
    endpoint += `&${part}`
    return endpoint;
  });
  readonly data = computed(() => this.result.value()?.value ?? []);
  readonly total = computed(() => this.result.value()?.['@odata.count'] ?? 0);
  readonly loading = computed(() => this.result.isLoading());

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #grid = inject(FlexiGridService);

  constructor(){
    this.#breadcrumb.reset(this.bredcrumbs());
  }

  dataStateChange(state: StateModel){
    this.state.set(state);
  }
}