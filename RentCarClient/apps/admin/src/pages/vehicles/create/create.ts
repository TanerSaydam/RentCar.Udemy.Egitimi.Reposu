import { ChangeDetectionStrategy, Component, computed, inject, linkedSignal, resource, signal, ViewEncapsulation, ElementRef, viewChild, Signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgxMaskDirective } from 'ngx-mask';
import { lastValueFrom } from 'rxjs';
import Blank from 'apps/admin/src/components/blank/blank';
import { VehicleModel, initialVehicleModel } from '@shared/lib/models/vehicle.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { HttpService } from '@shared/lib/services/http';
import { DatePipe, NgClass } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { FlexiSelectModule } from 'flexi-select';
import { ODataModel } from '@shared/lib/models/odata.model';
import { CategoryModel } from '@shared/lib/models/category.model';
import { BranchModel } from '@shared/lib/models/branch.model';

export interface FeatureGroup {
    group: string;
    features: { key: string; label: string; icon: string }[];
}

export const brandList = [
    'Toyota',
    'Renault',
    'Volkswagen',
    'Ford', 'Fiat',
    'Hyundai',
    'Peugeot',
    'Opel',
    'Honda',
    'BMW'
  ];

  export const colorList = [
    'Beyaz',
    'Siyah',
    'Gri',
    'Kırmızı',
    'Mavi',
    'Yeşil',
    'Sarı',
    'Turuncu',
    'Kahverengi',
    'Mor'
  ];

  export const fuelTypeList = [
    'Benzin',
    'Dizel',
    'LPG',
    'Elektrik',
    'Hibrit'
  ];

  export const transmissionList = [
    'Manuel',
    'Otomatik',
    'CVT'
  ];

  export const modelYearList = Array.from({length: 16}, (_, i) => 2010 + i);

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgClass,
    NgxMaskDirective,
    FlexiSelectModule
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DatePipe]
})
export default class CreateVehicle {
  readonly id = signal<string | undefined>(undefined);
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Araçlar',
      icon: 'bi-car-front',
      url: '/vehicles'
    }
  ]);
  readonly brandList = computed(() => brandList);
  readonly modelYearList = computed(() => modelYearList);
  readonly colorList = computed(() => colorList);
  readonly fuelTypeList = (() => fuelTypeList);
  readonly transmissionList = (() => transmissionList);

  readonly seatCountList = [
    { value: 2, label: '2 Kişi' },
    { value: 4, label: '4 Kişi' },
    { value: 5, label: '5 Kişi' },
    { value: 7, label: '7 Kişi' },
    { value: 8, label: '8 Kişi' },
    { value: 9, label: '9+ Kişi' }
  ];

  readonly featureGroups: FeatureGroup[] = [
    {
      group: 'Güvenlik Özellikleri',
      features: [
        { key: 'Airbag', label: 'Airbag', icon: 'bi-shield' },
        { key: 'ABS', label: 'ABS', icon: 'bi-shield-exclamation' },
        { key: 'ESP', label: 'ESP', icon: 'bi-shield-check' },
        { key: 'Alarm Sistemi', label: 'Alarm Sistemi', icon: 'bi-bell' }
      ]
    },
    {
      group: 'Sürüş Destekleri',
      features: [
        { key: 'GPS Navigasyon', label: 'GPS Navigasyon', icon: 'bi-geo-alt' },
        { key: 'Park Sensörü', label: 'Park Sensörü', icon: 'bi-broadcast-pin' },
        { key: 'Geri Görüş Kamerası', label: 'Geri Görüş Kamerası', icon: 'bi-camera-video' },
        { key: 'Cruise Control', label: 'Cruise Control', icon: 'bi-speedometer2' }
      ]
    },
    {
      group: 'Konfor Özellikleri',
      features: [
        { key: 'Klima', label: 'Klima', icon: 'bi-snow' },
        { key: 'Isıtmalı Koltuk', label: 'Isıtmalı Koltuk', icon: 'bi-thermometer-half' },
        { key: 'Sunroof', label: 'Sunroof', icon: 'bi-brightness-high' },
        { key: 'Bluetooth', label: 'Bluetooth', icon: 'bi-bluetooth' }
      ]
    },
    {
      group: 'Multimedya',
      features: [
        { key: 'Dokunmatik Ekran', label: 'Dokunmatik Ekran', icon: 'bi-tablet' },
        { key: 'USB Bağlantısı', label: 'USB Bağlantısı', icon: 'bi-usb' },
        { key: 'Premium Ses Sistemi', label: 'Premium Ses Sistemi', icon: 'bi-music-note-beamed' },
        { key: 'Apple CarPlay', label: 'Apple CarPlay', icon: 'bi-phone' }
      ]
    }
  ];

  readonly insuranceTypeList = signal<string[]>([
    'Kasko & Sigorta',
    'Sigorta'
  ]);

  readonly tractionTypeList = [
    'Önden Çekiş',
    'Arkadan İtiş',
    '4x4',
    'AWD',
    'Diğer'
  ];

  readonly tireStatusList = [
    'Yeni',
    'İyi',
    'Orta',
    'Zayıf',
    'Değişmeli'
  ];

  readonly generalStatusList = [
    'Çok İyi',
    'İyi',
    'Orta',
    'Bakım Gerekli',
    'Hasarlı'
  ];

  readonly pageTitle = computed(() => this.id() ? 'Araç Güncelle' : 'Araç Ekle');
  readonly pageIcon = computed(() => this.id() ? 'bi-pen' : 'bi-plus');
  readonly btnName = computed(() => this.id() ? 'Güncelle' : 'Kaydet');
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      if (!this.id()) return null;
      const res = await lastValueFrom(this.#http.getResource<VehicleModel>(`/rent/vehicles/${this.id()}`));
      this.bredcrumbs.update(prev => [...prev, {
        title: res.data!.brand + ' ' + res.data!.model,
        icon: 'bi-pen',
        url: `/vehicles/edit/${this.id()}`,
        isActive: true
      }]);
      this.#breadcrumb.reset(this.bredcrumbs());
      return res.data;
    }
  });
  readonly data = linkedSignal(() => this.result.value() ?? { ...initialVehicleModel });
  readonly loading = linkedSignal(() => this.result.isLoading());
  readonly categoryResource = httpResource<ODataModel<CategoryModel>>(() => '/rent/odata/categories');
  readonly categoryList = computed(() => this.categoryResource.value()?.value ?? []);
  readonly categoryLoading = computed(() => this.categoryResource.isLoading());

  readonly branchResource = httpResource<ODataModel<BranchModel>>(() => '/rent/odata/branches');
  readonly branchList = computed(() => this.branchResource.value()?.value ?? []);
  readonly branchLoading = computed(() => this.branchResource.isLoading());
  featuresInput = '';
  readonly file = signal<any | undefined>(undefined);
  readonly fileData = signal<string | undefined>(undefined);

  readonly fileInput = viewChild.required<ElementRef<HTMLInputElement>>('fileInput');
  dragOver = false;

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);
  readonly #date = inject(DatePipe);

  constructor() {
    this.#activated.params.subscribe(res => {
      if (res['id']) {
        this.id.set(res['id']);
      } else {
        this.bredcrumbs.update(prev => [...prev, {
          title: 'Ekle',
          icon: 'bi-plus',
          url: '/vehicles/add',
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());

        const date = this.#date.transform(new Date(),"yyyy-MM-dd")!;
        this.data.update(prev => ({
            ...prev,
            cascoEndDate: date,
            inspectionDate: date,
            insuranceEndDate: date,
            lastMaintenanceDate: date,
        }));
      }
    });
  }

  save(form: NgForm) {
    if (!form.valid) return;

    this.loading.set(true);

    // FormData oluştur
    const formData = new FormData();
    const d = this.data();

    // Tüm alanları ekle
    formData.append('Brand', d.brand);
    formData.append('Model', d.model);
    formData.append('ModelYear', d.modelYear.toString());
    formData.append('Color', d.color);
    formData.append('Plate', d.plate);
    formData.append('CategoryId', d.categoryId);
    formData.append('BranchId', d.branchId);
    formData.append('VinNumber', d.vinNumber);
    formData.append('EngineNumber', d.engineNumber);
    formData.append('Description', d.description);
    formData.append('FuelType', d.fuelType);
    formData.append('Transmission', d.transmission);
    formData.append('EngineVolume', d.engineVolume?.toString() ?? '0');
    formData.append('EnginePower', d.enginePower?.toString() ?? '0');
    formData.append('TractionType', d.tractionType);
    formData.append('FuelConsumption', d.fuelConsumption?.toString() ?? '0');
    formData.append('SeatCount', d.seatCount?.toString() ?? '0');
    formData.append('Kilometer', d.kilometer?.toString() ?? '0');
    formData.append('DailyPrice', d.dailyPrice?.toString() ?? '0');
    formData.append('WeeklyDiscountRate', d.weeklyDiscountRate?.toString() ?? '0');
    formData.append('MonthlyDiscountRate', d.monthlyDiscountRate?.toString() ?? '0');
    formData.append('InsuranceType', d.insuranceType);
    formData.append('LastMaintenanceDate', d.lastMaintenanceDate);
    formData.append('LastMaintenanceKm', d.lastMaintenanceKm?.toString() ?? '0');
    formData.append('NextMaintenanceKm', d.nextMaintenanceKm?.toString() ?? '0');
    formData.append('InspectionDate', d.inspectionDate);
    formData.append('InsuranceEndDate', d.insuranceEndDate);
    formData.append('CascoEndDate', d.cascoEndDate);
    formData.append('TireStatus', d.tireStatus);
    formData.append('GeneralStatus', d.generalStatus);
    formData.append('IsActive', d.isActive ? 'true' : 'false');

    // Features (array)
    if (d.features && d.features.length > 0) {
      d.features.forEach(f => formData.append('Features', f));
    }

    // Resim dosyası (fileInput ile seçilen dosya)
    if (this.file()) {
      formData.append('File', this.file(),this.file().name);
    }

    // Güncellemede Id ekle
    if (this.id()) {
      formData.append('Id', this.id()!);
    }

    const endpoint = this.id() ? '/rent/vehicles' : '/rent/vehicles';
    const method = this.id() ? this.#http.put<string> : this.#http.post<string>;

    method.call(this.#http, endpoint, formData, res => {
      this.#toast.showToast("Başarılı", res, this.id() ? "info" : "success");
      this.#router.navigateByUrl("/vehicles");
      this.loading.set(false);
    }, () => this.loading.set(false));
  }

  changeStatus(status: boolean) {
    this.data.update(prev => ({
      ...prev,
      isActive: status
    }));
  }

  addFeature() {
    if (this.featuresInput.trim()) {
      this.data.update(prev => ({
        ...prev,
        features: [...prev.features, this.featuresInput.trim()]
      }));
      this.featuresInput = '';
    }
  }

  removeFeature(index: number) {
    this.data.update(prev => ({
      ...prev,
      features: prev.features.filter((_, i) => i !== index)
    }));
  }

  triggerFileInput() {
    const fileInput = this.fileInput();
    fileInput.nativeElement.value = ''; // Aynı dosya tekrar seçilebilsin diye sıfırla
    fileInput.nativeElement.click();
  }

  onImageChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.file.set(file);
      const reader = new FileReader();
      reader.onload = () => {
        this.fileData.set(reader.result as string)
        this.data.update(prev => ({
          ...prev,
          imageUrl: ''
        }));
      };
      reader.readAsDataURL(file);
      input.value = ''; // Aynı dosya tekrar seçilebilsin diye sıfırla
    }
  }

  onImageDragOver(event: DragEvent) {
    event.preventDefault();
    this.dragOver = true;
  }

  onImageDragLeave(event: DragEvent) {
    event.preventDefault();
    this.dragOver = false;
  }

  onImageDrop(event: DragEvent) {
    event.preventDefault();
    this.dragOver = false;
    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      const file = event.dataTransfer.files[0];
      this.file.set(file);
      const reader = new FileReader();
      reader.onload = () => {
        this.fileData.set(reader.result as string)
        this.data.update(prev => ({
          ...prev,
          imageUrl: ''
        }));
      };
      reader.readAsDataURL(file);
      const fileInput = this.fileInput();
      if (fileInput) fileInput.nativeElement.value = '';
    }
  }

  // Özellik seçimini yönetir
  toggleFeature(feature: string) {
    const features = this.data().features;
    if (features.includes(feature)) {
      this.data.update(prev => ({
        ...prev,
        features: prev.features.filter(f => f !== feature)
      }));
    } else {
      this.data.update(prev => ({
        ...prev,
        features: [...prev.features, feature]
      }));
    }
  }

  // Özellik seçili mi kontrolü
  isFeatureSelected(feature: string): boolean {
    return this.data().features.includes(feature);
  }

  showImageUrl(){
    if(this.fileData()){
        return this.fileData()
    }else if(this.data().imageUrl){
        return `https://localhost:7207/images/${this.data().imageUrl}`
    }else{
        return '/no-noimage.png'
    }
  }
}