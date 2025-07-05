import { Location } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, effect, inject, linkedSignal, signal, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Result } from 'apps/admin/src/models/result.model';
import { initialRole, RoleModel } from 'apps/admin/src/models/role.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiTreeNode, FlexiTreeviewComponent, FlexiTreeviewService } from 'flexi-treeview';

@Component({
  imports: [
    FlexiTreeviewComponent
  ],
  templateUrl: './permissions.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Permissions {
  readonly id = signal<string>("");
  readonly roleResult = httpResource<Result<RoleModel>>(() => `/rent/roles/${this.id()}`);
  readonly role = computed(() => this.roleResult.value()?.data ?? initialRole);
  readonly treeviewTitle = computed(() => this.role().name + " İzinleri")
  readonly result = httpResource<Result<string[]>>(() => '/rent/permissions');
  readonly data = computed(() => {
    const data = this.result.value()?.data ?? [];
    const nodes = data.map(val => {
      var parts = val.split(":");
      var data = {id: val, code: parts[0], name: parts[1]}

      return data;
    })
    const treeNodes: FlexiTreeNode[] = this.#treeview.convertToTreeNodes(nodes,"id","code","name");

    treeNodes.forEach(val => {
      val.children?.forEach(el => {
        el.selected = this.role().permissions.includes(el.originalData.id);
        el.name = this.capitalizeFirstLetter(el.name);
      });

      val.selected = !val.children?.some(val => !val.selected);
      val.indeterminate = !!val.children?.some(child => child.selected) &&
                          !val.children?.every(child => child.selected);

      val.name = this.capitalizeFirstLetter(val.name);
    });

    return treeNodes;
  });
  readonly loading = computed(() => this.result.isLoading());
  readonly rolePermission = linkedSignal<{roleId: string, permissions: string[]}>(() => ({
    roleId: this.id(),
    permissions: []
  }));

  readonly breadcrumbs = signal<BreadcrumbModel[]>([]);

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #treeview = inject(FlexiTreeviewService);
  readonly #http = inject(HttpService);
  readonly #location = inject(Location);

  constructor(){
    this.#activated.params.subscribe(res => {
      this.id.set(res['id']);
    });

    effect(() => {
      this.breadcrumbs.set([
        {
          title: 'Roller',
          icon: 'bi-clipboard2-check',
          url: '/roles',
        },
        {
          title: this.role().name + " İzinleri",
          icon: "bi-key",
          url: `/roles/permissions/${this.id()}`,
          isActive: true
        }
      ]);

      this.#breadcrumb.reset(this.breadcrumbs());
    })
  }

  onSelected(event:any){
    this.rolePermission.update(prev => ({
      ...prev,
      permissions: event.map((val:any) => val.id)
    }));
  }

  update(){
    this.#http.put('/rent/roles/update-permissions', this.rolePermission(), res => {
      this.#location.back();
    });
  }

  capitalizeFirstLetter(text: string): string {
    if (!text) return '';
    return text.charAt(0).toUpperCase() + text.slice(1);
  }
}