import { ChangeDetectionStrategy, Component, inject, OnInit, resource, ViewEncapsulation } from '@angular/core';
import { BreadcrumbService } from '../../services/breadcrumb';
import Blank from '../../components/blank/blank';
import { HttpClient, httpResource } from '@angular/common/http';
import { HttpService } from '../../services/http';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank
  ],
  templateUrl: './dashboard.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Dashboard implements OnInit {
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly result = resource({
    loader: async() => {
      var res = await lastValueFrom(this.#http.getResource("/rent/"));
      return res;
    }
  });

  readonly #http = inject(HttpService);

  ngOnInit(): void {
    this.#breadcrumb.setDashboard();
  }

  makeRequest(){
    this.result.reload();
  }
}
