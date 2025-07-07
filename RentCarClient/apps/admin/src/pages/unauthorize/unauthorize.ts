import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  imports: [
    RouterLink
  ],
  templateUrl: './unauthorize.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Unatuhorize {

}
