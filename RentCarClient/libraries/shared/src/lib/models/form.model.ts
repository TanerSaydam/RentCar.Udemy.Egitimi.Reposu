export interface FormModel {
  reservationId: string;
  reservationNumber: string;
  reservationStatus: string;
  pickUpDateTime: string;
  deliveryDateTime: string;
  kilometer: number;
  customerId: string;
  customer: {
    fullName: string;
    identityNumber: string;
    phoneNumber: string;
    email: string;
    fullAddress: string;
  };
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
  supplies: string[];
  imageUrls: string[];
  damages: {
    level: string;
    description: string;
  }[];
  note: string;
  files:any[];
}

export const initialForm: FormModel = {
  reservationId: '',
  reservationNumber: '',
  reservationStatus: '',
  pickUpDateTime: '',
  deliveryDateTime: '',
  kilometer: 0,
  customerId: '',
  customer: {
    fullName: '',
    identityNumber: '',
    phoneNumber: '',
    email: '',
    fullAddress: ''
  },
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
  supplies: [],
  imageUrls: [],
  damages: [],
  note: '',
  files: []
};