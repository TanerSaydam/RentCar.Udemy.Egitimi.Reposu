import { EntityModel } from './entity.model';

export interface CustomerModel extends EntityModel {
  firstName: string;
  lastName: string;
  fullName: string;
  identityNumber: string;
  dateOfBirth: string;
  phoneNumber: string;
  email: string;
  drivingLicenseIssuanceDate: string;
  fullAddress: string;
}

export const initialCustomerModel: CustomerModel = {
  id: '',
  firstName: '',
  lastName: '',
  fullName: '',
  identityNumber: '',
  dateOfBirth: '',
  phoneNumber: '',
  email: '',
  drivingLicenseIssuanceDate: '',
  fullAddress: '',
  isActive: true,
  createdAt: '',
  createdBy: '',
  createdFullName: '',
};