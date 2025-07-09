import { DatePipe, Location } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, ElementRef, inject, linkedSignal, resource, signal, viewChild, ViewEncapsulation } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import Loading from 'apps/admin/src/components/loading/loading';
import { FormModel, initialForm } from 'apps/admin/src/models/form.model';
import { Result } from 'apps/admin/src/models/result.model';
import { NgxMaskPipe } from 'ngx-mask';
import { FlexiGridModule } from "flexi-grid";
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiToastService } from 'flexi-toast';

@Component({
  imports: [
    Loading,
    DatePipe,
    NgxMaskPipe,
    FormsModule,
    RouterLink,
    FlexiGridModule
  ],
  templateUrl: './form.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Form {
  readonly reservationId = signal<string>("");
  readonly type = signal<string>("");
  readonly result = httpResource<Result<FormModel>>(() => `/rent/reservation-form/${this.reservationId()}/${this.type()}`);
  readonly data = linkedSignal(() => this.result.value()?.data ?? { ...initialForm });
  readonly loading = linkedSignal(() => this.result.isLoading());
  readonly pageTitle = computed(() => this.type() === 'pickup' ? 'ARAÇ TESLİM FORMU' : 'ARAÇ İADE FORMU');
  readonly btnName = computed(() => this.type() === 'pickup' ? 'Teslimi Tamamla' : 'İadeyi Tamamla');
  readonly images = signal<any[]>([]);
  readonly customerApproval = signal<boolean>(false);
  readonly showButton = computed(() => {
    if(this.type() === 'pickup' && this.data().reservationStatus === 'Bekliyor') return true;
    if(this.type() === 'delivery' && this.data().reservationStatus === 'Teslim Edildi') return true;
    return false;
  })
  readonly damage = signal<{
    level: string;
    description: string;
  }>({ level: 'Küçük Çizik', description: '' })

  readonly supplies = signal<{ "icon": string; "name": string }[]>([
    { icon: 'bi bi-circle', name: 'Stepne Lastik' },
    { icon: 'bi bi-tools', name: 'Kriko' },
    { icon: 'bi bi-triangle', name: 'Reflektör Üçgen' },
    { icon: 'bi bi-heart-pulse', name: 'İlk Yardım Çantası' },
    { icon: 'bi bi-fire', name: 'Yangın Söndürücü' },
    { icon: 'bi bi-outlet', name: 'Takviye Kablosu' },
    { icon: 'bi bi-file-earmark-text', name: 'Araç Evrakları' },
    { icon: 'bi bi-key', name: 'Yedek Anahtar' },
    { icon: 'bi bi-book', name: 'Kullanım Kılavuzu' },
    { icon: 'bi bi-phone', name: 'Telefon Şarj Aleti' }
  ]);

  readonly fileInput = viewChild<ElementRef<HTMLInputElement>>("fileInput");

  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #location = inject(Location);

  constructor() {
    this.#activated.params.subscribe(res => {
      this.reservationId.set(res['reservationId']);
      this.type.set(res['type']);
    });
  }

  isHaveSupplies(name: string) {
    return this.data().supplies.some(i => i == name);
  }

  toggleSupply(name: string) {
    if(!this.showButton()) return;
    const supplies = [...this.data().supplies];
    const index = supplies.findIndex(i => i == name);
    if (index === -1) {
      supplies.push(name);
    } else {
      supplies.splice(index, 1);
    }

    this.data.update(prev => ({ ...prev, supplies: [...supplies] }));
  }

  deleteImageUrl(index: number) {
    const images = [...this.data().imageUrls];
    images.splice(index, 1);
    this.data.update(prev => ({ ...prev, imageUrls: [...images] }));
  }

  deleteFiles(index: number) {
    const files = [...this.data().files];
    files.splice(index, 1);
    const images = [...this.images()];
    images.splice(index, 1);
    this.images.set([...images]);
    this.data.update(prev => ({ ...prev, files: [...files] }));
  }

  openFileInput() {
    this.fileInput()!.nativeElement.click();
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.data.update(prev => ({ ...prev, files: [...prev.files ?? [], file] }))
      const reader = new FileReader();
      reader.onload = () => {
        this.images.update(prev => [...prev, reader.result as string]); // Base64 olarak diziye ekleniyor
      };
      reader.readAsDataURL(file);
    }
  }

  addDamage() {
    const damages = [...this.data().damages];
    damages.push(this.damage());
    this.damage.set({ level: 'Küçük Çizik', description: '' });
    this.data.update(prev => ({ ...prev, damages: [...damages] }));
  }

  removeDamage(index: number) {
    const damages = [...this.data().damages];
    damages.splice(index, 1);
    this.data.update(prev => ({ ...prev, damages: [...damages] }));
  }

  setDamageClass(level: string) {
    switch (level) {
      case 'Küçük Çizik': return 'minor-damage'
      case 'Büyük Hasar': return 'major-damage'
      case 'Kritik Hasar': return 'critical-damage'
      default: return '';
    }
  }

  save() {
    const totalImage = this.data().files?.length ?? 0;
    if (totalImage === 0) {
      this.#toast.showToast("Hata", "Resim eklemediniz", "error");
      return;
    }

    if (this.data().kilometer < 0) {
      this.#toast.showToast("Hata", "Araç KM 0 dan küçük olamaz", "error");
      return;
    }

    if(!this.customerApproval() && this.type() === "pickup"){
      this.#toast.showToast("Hata", "Formu onaylamalısınız", "error");
      return;
    }

    const formData = new FormData();
    formData.append("ReservationId", this.data().reservationId);
    formData.append("Type", this.type());
    formData.append("Kilometer", this.data().kilometer.toString());
    this.data().files.forEach(val => {
      formData.append("Files", val, val.name);
    });
    this.data().supplies.forEach(val => {
      formData.append("Supplies", val);
    });
    this.data().damages.forEach((val, index) => {
      formData.append(`Damages[${index}].level`, val.level);
      formData.append(`Damages[${index}].description`, val.description);
    });
    formData.append("Note", this.data().note);

    this.loading.set(true);
    this.#http.put<string>(`/rent/reservation-form`, formData, res => {
      this.#location.back();
      this.#toast.showToast("Başarılı", res, "info");
      this.loading.set(false);
    }, () => this.loading.set(false));
  }
}