import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  imports: [RouterModule],
  selector: 'app-root',
  template: '<router-outlet />',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class App {
}
