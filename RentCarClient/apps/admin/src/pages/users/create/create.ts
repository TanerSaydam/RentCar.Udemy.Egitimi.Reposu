import { NgClass } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, inject, linkedSignal, resource, signal, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import { BranchModel } from '@shared/lib/models/branch.model';
import { ODataModel } from '@shared/lib/models/odata.model';
import { RoleModel } from '@shared/lib/models/role.model';
import { UserModel, initialUser } from '@shared/lib/models/user.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { Common } from 'apps/admin/src/services/common';
import { HttpService } from '@shared/lib/services/http';
import { FlexiSelectModule } from 'flexi-select';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgClass,
    FlexiSelectModule
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Create {
  readonly id = signal<string | undefined>(undefined);
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Kullanıcılar',
      icon: 'bi-people',
      url: '/users'
    }
  ]);
  readonly pageTitle = computed(() => this.id() ? 'Kullanıcı Güncelle' : 'Kullanıcı Ekle');
  readonly pageIcon = computed(() => this.id() ? 'bi-pen' : 'bi-plus');
  readonly btnName = computed(() => this.id() ? 'Güncelle' : 'Kaydet');
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      var res = await lastValueFrom(this.#http.getResource<UserModel>(`/rent/users/${this.id()}`));

      this.bredcrumbs.update(prev => [...prev, {
          title: res.data!.fullName,
          icon: 'bi-pen',
          url: `/users/edit/${this.id()}`,
          isActive: true
      }]);
      this.#breadcrumb.reset(this.bredcrumbs());
      return res.data;
    }
  });
  readonly data = linkedSignal(() => this.result.value() ?? {...initialUser});
  readonly loading = linkedSignal(() => this.result.isLoading());
  readonly branchResult = httpResource<ODataModel<BranchModel>>(() => '/rent/odata/branches');
  readonly branches = computed(() => this.branchResult.value()?.value ?? []);
  readonly branchLoading = computed(() => this.branchResult.isLoading());
  readonly roleResult = httpResource<ODataModel<RoleModel>>(() => '/rent/odata/roles');
  readonly roles = computed(() => this.roleResult.value()?.value ?? []);
  readonly roleLoading = computed(() => this.roleResult.isLoading());

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);
  readonly #common = inject(Common);

  constructor() {
    this.#activated.params.subscribe(res => {
      if (res['id']) {
        this.id.set(res['id']);
      } else {
        this.bredcrumbs.update(prev => [...prev, {
          title: 'Ekle',
          icon: 'bi-plus',
          url: '/users/add',
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    })
  }

  save(form: NgForm){
    if(!form.valid) return;

    if(!this.id()){
      this.loading.set(true);
      this.#http.post<string>('/rent/users', this.data(), (res) => {
        this.#toast.showToast("Başarılı",res,"success");
        this.#router.navigateByUrl("/users");
        this.loading.set(false);
      },() => this.loading.set(false));
    }else{
      this.loading.set(true);
      this.#http.put<string>('/rent/users', this.data(), (res) => {
        this.#toast.showToast("Başarılı",res,"info");
        this.#router.navigateByUrl("/users");
        this.loading.set(false);
      },() => this.loading.set(false));
    }
  }

  changeStatus(status:boolean){
    this.data.update(prev => ({
      ...prev,
      isActive: status
    }));
  }

  checkIsAdmin(){
    return this.#common.decode().role === "sys_admin";
  }
}
