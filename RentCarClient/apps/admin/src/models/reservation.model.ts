import { EntityModel } from "./entity.model";

export interface ReservationModel extends EntityModel {
  reservationNumber: string;
  customerId: string;
  customer: {
    fullName: string;
    identityNumber: string;
    phoneNumber: string;
    email: string;
    fullAddress: string;
  };
  pickUpLocationId?: string;
  pickUp: {
    name: string;
    fullAddress: string;
    phoneNumber: string;
  };
  pickUpDate: string;
  pickUpTime: string;
  pickUpDateTime: string;
  deliveryDate: string;
  deliveryTime: string;
  deliveryDateTime: string;
  vehicleId: string;
  vehicleDailyPrice: number;
  vehicle: {
    id: string;
    brand: string;
    model: string;
    modelYear: number;
    color: string;
    categoryName: string;
    fuelConsumption: number;
    seatCount: number;
    tractionType: string;
    kilometer: number;
    imageUrl: string;
    plate: string;
  };
  protectionPackageId: string;
  protectionPackagePrice: number;
  protectionPackageName: string;
  reservationExtras: {
    extraId: string;
    extraName: string;
    price: number;
  }[];
  note: string;
  total: number;
  status: string;
  totalDay: number;
  creditCartInformation: {
    cartNumber: string,
    owner:string;
    expiry: string;
    ccv: string;
  },
  paymentInformation: {
    cartNumber: string,
    owner:string;
  },
  histories: {title: string, description:string, createdAt: string}[]
}

export const initialReservation: ReservationModel = {
  reservationNumber: '',
  customerId: '',
  customer: {
    fullName: '',
    identityNumber: '',
    phoneNumber: '',
    email: '',
    fullAddress: ''
  },
  pickUp: {
    name: '',
    fullAddress: '',
    phoneNumber: ''
  },
  pickUpDate: '',
  pickUpTime: '09:00:00',
  pickUpDateTime: '',
  deliveryDate: '',
  deliveryTime: '09:00:00',
  deliveryDateTime: '',
  vehicleId: '',
  vehicleDailyPrice: 0,
  vehicle: {
    id: '',
    brand: '',
    model: '',
    modelYear: 0,
    color: '',
    categoryName: '',
    fuelConsumption: 0,
    seatCount: 0,
    tractionType: '',
    kilometer: 0,
    imageUrl: '',
    plate: ''
  },
  protectionPackageId: '',
  protectionPackagePrice: 0,
  protectionPackageName: '',
  reservationExtras: [],
  note: '',
  total: 0,
  status: '',
  totalDay: 0,
  creditCartInformation: {
    cartNumber: '',
    owner: '',
    expiry: '',
    ccv: ''
  },
  paymentInformation: {
    cartNumber: '',
    owner: '',
  },
  histories: [],
  id: '',
  isActive: true,
  createdAt: '',
  createdBy: '',
  createdFullName: ''
};