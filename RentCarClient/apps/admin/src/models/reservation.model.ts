import { EntityModel } from "./entity.model";

export interface ReservationModel extends EntityModel {
  customerId: string;
  customer: {
    fullName: string;
    identityNumber: string;
    phoneNumber: string;
    email: string;
    fullAddress: string;
  };
  pickUpLocationId: string;
  pickUp: {
    name: string;
    fullAddress: string;
    phoneNumber: string;
  };
  pickUpDate: string;
  pickUpTime: string;
  deliveryDate: string;
  deliveryTime: string;
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
}

export const initialReservation: ReservationModel = {
  customerId: '',
  customer: {
    fullName: '',
    identityNumber: '',
    phoneNumber: '',
    email: '',
    fullAddress: ''
  },
  pickUpLocationId: '',
  pickUp: {
    name: '',
    fullAddress: '',
    phoneNumber: ''
  },
  pickUpDate: '',
  pickUpTime: '',
  deliveryDate: '',
  deliveryTime: '',
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
    kilometer: 0
  },
  protectionPackageId: '',
  protectionPackagePrice: 0,
  protectionPackageName: '',
  reservationExtras: [],
  note: '',
  total: 0,
  status: '',
  totalDay: 0,
  id: '',
  isActive: true,
  createdAt: '',
  createdBy: '',
  createdFullName: ''
};