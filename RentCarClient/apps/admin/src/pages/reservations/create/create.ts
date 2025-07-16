import { DatePipe, NgClass, NgTemplateOutlet } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, effect, inject, linkedSignal, resource, signal, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import { BranchModel } from '@shared/lib/models/branch.model';
import { CategoryModel } from '@shared/lib/models/category.model';
import { CustomerModel, initialCustomerModel } from '@shared/lib/models/customer.model';
import { ODataModel } from '@shared/lib/models/odata.model';
import { initialReservation, ReservationModel } from '@shared/lib/models/reservation.model';
import { initialVehicleModel, VehicleModel } from '@shared/lib/models/vehicle.model';
import { BreadcrumbModel, BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { Common } from 'apps/admin/src/services/common';
import { HttpService } from '@shared/lib/services/http';
import { FlexiGridModule, FlexiGridService, StateModel } from 'flexi-grid';
import { FlexiPopupModule } from 'flexi-popup';
import { FlexiSelectModule } from 'flexi-select';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { lastValueFrom } from 'rxjs';
import { TrCurrencyPipe } from 'tr-currency';
import { fuelTypeList, transmissionList } from '../../vehicles/create/create';
import { VehiclePipe } from 'apps/admin/src/pipes/vehicle-pipe';
import { ProtectionPackageModel } from '@shared/lib/models/protection-package.model';
import { ExtraModel } from '@shared/lib/models/extra.model';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgxMaskDirective,
    NgxMaskPipe,
    NgClass,
    FlexiPopupModule,
    FlexiGridModule,
    NgxMaskPipe,
    NgTemplateOutlet,
    FlexiSelectModule,
    DatePipe,
    TrCurrencyPipe,
    VehiclePipe
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DatePipe]
})
export default class Create {
  readonly id = signal<string | undefined>(undefined);
  readonly bredcrumbs = signal<BreadcrumbModel[]>([
    {
      title: 'Rezervasyonlar',
      icon: 'bi-calendar-check',
      url: '/reservations'
    }
  ]);
  readonly pageTitle = computed(() => this.id() ? 'Rezervasyon Güncelle' : 'Rezervasyon Ekle');
  readonly pageIcon = computed(() => this.id() ? 'bi-pen' : 'bi-plus');
  readonly btnName = computed(() => this.id() ? 'Rezervasyonu Güncelle' : 'Rezervasyon Oluştur');
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      if (!this.id()) return null;
      const res = await lastValueFrom(this.#http.getResource<ReservationModel>(`/rent/reservations/${this.id()}`));
      this.bredcrumbs.update(prev => [...prev, {
        title: res.data!.customer.fullName,
        icon: 'bi-pen',
        url: `/reservation/edit/${this.id()}`,
        isActive: true
      }]);
      this.#breadcrumb.reset(this.bredcrumbs());

      const customer = res.data!.customer;
      this.selectedCustomer.set({
        ...initialCustomerModel,
        id: res.data!.customerId,
        fullName: customer.fullName,
        fullAddress: customer.fullAddress,
        phoneNumber: customer.phoneNumber,
        email: customer.email });

        const vehicle = res.data!.vehicle;
        this.selectedVehicle.set({
          ...initialVehicleModel,
          id: vehicle.id,
          brand: vehicle.brand,
          model: vehicle.model,
          modelYear: vehicle.modelYear,
          color: vehicle.color,
          categoryName: vehicle.categoryName,
          fuelConsumption: vehicle.fuelConsumption,
          seatCount: vehicle.seatCount,
          tractionType: vehicle.tractionType,
          kilometer: vehicle.kilometer,
          imageUrl: vehicle.imageUrl,
          dailyPrice: res.data!.vehicleDailyPrice,
        });

        this.vehicles.set([{...this.selectedVehicle()!}]);

        let totalExtra = 0;
        res.data!.reservationExtras.forEach(val => {
          totalExtra += (val.price * res.data!.totalDay)
        });
        this.totalExtra.set(totalExtra);
      return res.data;
    }
  });
  readonly data = linkedSignal(() => this.result.value() ?? { ...initialReservation });
  readonly loading = linkedSignal(() => this.result.isLoading());
  isCustomerPopupVisible = false;
  readonly isCustomerPopupLoading = signal<boolean>(false);
  readonly customerPopupData = signal<CustomerModel>({ ...initialCustomerModel });
  readonly customerState = signal<StateModel>(new StateModel());
  readonly customersResult = httpResource<ODataModel<CustomerModel>>(() => {
    let endpoint = '/rent/odata/customers?count=true&';
    const part = this.#grid.getODataEndpoint(this.customerState());
    endpoint += part;
    return endpoint;
  });
  readonly customersData = computed(() => this.customersResult.value()?.value ?? []);
  readonly customersTotal = computed(() => this.customersResult.value()?.['@odata.count'] ?? 0);
  readonly customersLoading = computed(() => this.customersResult.isLoading());
  readonly selectedCustomer = signal<CustomerModel | undefined>(undefined);
  readonly branchResult = httpResource<ODataModel<BranchModel>>(() => '/rent/odata/branches');
  readonly branchesData = computed(() => this.branchResult.value()?.value ?? []);
  readonly branchesLoading = computed(() => this.branchResult.isLoading());
  readonly isAdmin = computed(() => this.#common.decode().role === 'sys_admin');
  readonly timeData = signal<string[]>(
    Array.from({ length: 31 }, (_, i) => {
      const hour = 9 + Math.floor(i / 2);
      const minute = i % 2 === 0 ? "00" : "30";
      return `${hour.toString().padStart(2, "0")}:${minute}`;
    })
  );
  readonly branchName = linkedSignal(() => this.#common.decode().branch);
  readonly vehicles = signal<VehicleModel[]>([]);
  readonly vehicleLoading = signal<boolean>(false);
  readonly categoryResult = httpResource<ODataModel<CategoryModel>>(() => '/rent/odata/categories');
  readonly categoriesData = computed(() => this.categoryResult.value()?.value ?? []);
  readonly categoriesLoading = computed(() => this.categoryResult.isLoading());
  readonly fuelTypeList = (() => fuelTypeList);
  readonly transmissionList = (() => transmissionList);
  readonly vehicleFilter = signal<{categoryName: string, fuelType: string, transmission: string}>({
    categoryName: '',
    fuelType: '',
    transmission: ''
  });
  readonly selectedVehicle = signal<VehicleModel | undefined>(undefined);
  readonly protectionPackageResult = httpResource<ODataModel<ProtectionPackageModel>>(()=> '/rent/odata/protection-packages?$orderby=OrderNumber');
  readonly protectionPackagesData = computed(() => this.protectionPackageResult.value()?.value ?? []);
  readonly protectionPackagesLoading = computed(() => this.protectionPackageResult.isLoading());
  readonly extraResult = httpResource<ODataModel<ExtraModel>>(()=> '/rent/odata/extras');
  readonly extrasData = computed(() => this.extraResult.value()?.value ?? []);
  readonly extrasLoading = computed(() => this.extraResult.isLoading());
  readonly totalExtra = signal<number>(0);

  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);
  readonly #date = inject(DatePipe);
  readonly #grid = inject(FlexiGridService);
  readonly #common = inject(Common);

  constructor() {
    this.#activated.params.subscribe(res => {
      if (res['id']) {
        this.id.set(res['id']);
      } else {
        this.bredcrumbs.update(prev => [...prev, {
          title: 'Ekle',
          icon: 'bi-plus',
          url: '/reservation/add',
          isActive: true
        }]);
        this.#breadcrumb.reset(this.bredcrumbs());
        const date = this.#date.transform("01.01.2000", "yyyy-MM-dd")!;
        this.customerPopupData.update(prev => ({ ...prev, dateOfBirth: date, drivingLicenseIssuanceDate: date }));
        const now = this.#date.transform(new Date(), "yyyy-MM-dd")!;
        const tomorrowDate = new Date();
        tomorrowDate.setDate(tomorrowDate.getDate() + 1);
        const tomorrow = this.#date.transform(tomorrowDate, "yyyy-MM-dd")!;

        this.data.update(prev => ({
          ...prev,
          pickUpDate: now,
          deliveryDate: tomorrow
        }));

        this.calculateDayDifference();
      }
    });
  }

  save(form: NgForm) {
    if (!form.valid) return;

    if(!this.data().protectionPackageId){
      this.#toast.showToast("Uyarı","Güvence paketi seçmelisiniz","error");
      return;
    }

    this.loading.set(true);
    if (!this.id()) {
      const cartInformation = {...this.data().creditCartInformation};
      cartInformation.ccv = cartInformation.ccv.toString();
      this.data.update(prev => ({
        ...prev,
        creditCartInformation: {...cartInformation}
      }));

      this.#http.post<string>('/rent/reservations', this.data(), res => {
        this.#toast.showToast("Başarılı", res, "success");
        this.#router.navigateByUrl("/reservations");
        this.loading.set(false);
      }, () => this.loading.set(false));
    } else {
      this.#http.put<string>('/rent/reservations', this.data(), res => {
        this.#toast.showToast("Başarılı", res, "info");
        this.#router.navigateByUrl("/reservations");
        this.loading.set(false);
      }, () => this.loading.set(false));
    }
  }

  saveCustomer(form: NgForm) {
    if (!form.valid) return;

    this.loading.set(true);
    this.#http.post<string>('/rent/customers', this.customerPopupData(), res => {
      this.#toast.showToast("Başarılı", res, "success");
      this.loading.set(false);
    }, () => this.loading.set(false));
  }

  customerDataStateChange(state: StateModel) {
    this.customerState.set(state);
  }

  selectCustomer(item: CustomerModel) {
    this.selectedCustomer.set(item);
    this.data.update(prev => ({ ...prev, customerId: item.id }));
  }

  clearCustomer() {
    this.selectedCustomer.set(undefined);
    this.data.update(prev => ({ ...prev, customerId: '' }));
  }

  calculateDayDifference() {
    this.vehicles.set([]);
    const pickUpDateTime = new Date(`${this.data().pickUpDate}T${this.data().pickUpTime}`);
    const deliveryDateTime = new Date(`${this.data().deliveryDate}T${this.data().deliveryTime}`);

    const diffMs = deliveryDateTime.getTime() - pickUpDateTime.getTime();

    if (diffMs <= 0){
      this.data.update(prev => ({...prev, totalDay: 0}));
      return;
    }

    const oneDayMs = 24 * 60 * 60 * 1000;

    const fullDays = Math.floor(diffMs / oneDayMs);
    const remainder = diffMs % oneDayMs;

    const totalDay = remainder > 0 ? fullDays + 1 : fullDays;
    this.data.update(prev => ({...prev, totalDay: totalDay}));
  }

  setLocation(id:any){
    const branch = this.branchesData().find(i => i.id == id)!;
    this.branchName.set(branch.name);
  }

  getVehicles(){
    const data = {
      branchId: !this.data().pickUpLocationId ? this.#common.decode().branchId : this.data().pickUpLocationId,
      pickUpDate: this.data().pickUpDate,
      pickUpTime: this.data().pickUpTime,
      deliveryDate: this.data().deliveryDate,
      deliverTime: this.data().deliveryTime
    }

    this.vehicleLoading.set(true);
    this.#http.post<VehicleModel[]>('/rent/reservations/vehicle-getall', data,(res) => {
      this.vehicles.set(res);
      this.vehicleLoading.set(false);
    },()=> this.vehicleLoading.set(false));
  }

  getVehicleImage(vehicle: VehicleModel){
    const endpoint = "https://localhost:7207/images/";
    return endpoint + vehicle.imageUrl;
  }

  selectVehicle(item:VehicleModel){
    this.selectedVehicle.set(item);
    this.data.update(prev => ({
      ...prev,
      vehicleId: item.id,
      vehicle: item,
      vehicleDailyPrice: item.dailyPrice,
    }));
    this.calculateTotal();
  }

  selectProtectionPackage(val: ProtectionPackageModel){
    if(val.id === this.data().protectionPackageId){
      this.data.update(prev => ({
        ...prev,
        protectionPackageId: '',
        protectionPackagePrice: 0,
        protectionPackageName: ''
      }));
    }else{
      this.data.update(prev => ({
        ...prev,
        protectionPackageId: val.id,
        protectionPackagePrice: val.price,
        protectionPackageName: val.name
      }));
    }
    this.calculateTotal();
  }

  selectExtra(val: ExtraModel){
    const extras = [...this.data().reservationExtras];
    const index = extras.findIndex(i => i.extraId === val.id);

    if (index !== -1) {
      extras.splice(index, 1);
    } else {
      extras.push({ extraId: val.id, price: val.price, extraName: val.name });
    }

    this.data.update(prev => ({ ...prev, reservationExtras: extras }));
    this.calculateTotal();
  }

  calculateTotal(){
    const totalVehicle = this.data().vehicleDailyPrice * this.data().totalDay;
    const totalProtectionpackage = this.data().protectionPackagePrice * this.data().totalDay;
    let totalExtra = 0;
    this.data().reservationExtras.forEach(val => {
      totalExtra += (val.price * this.data().totalDay)
    });
    this.totalExtra.set(totalExtra);
    const total = totalVehicle + totalProtectionpackage + totalExtra;
    this.data.update(prev => ({
      ...prev,
      total: total
    }));
  }

  checkedExtra(val: ExtraModel){
    return this.data().reservationExtras.some(i => i.extraId === val.id);
  }
}
