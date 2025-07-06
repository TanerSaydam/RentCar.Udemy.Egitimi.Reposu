import { EntityModel } from "./entity.model";

export interface BranchModel extends EntityModel{
    name: string;
    address: AddressModel;
    contact: ContactModel;
}

export interface AddressModel{
    city: string;
    district: string;
    fullAddress:string;
}

export interface ContactModel{
    phoneNumber1:string;
    phoneNumber2:string;
    email:string;
}

export const initialBranch: BranchModel = {
    id: '',
    name: '',
    address: {
        city: '',
        district: '',
        fullAddress: ''
    },
    contact: {
        email: '',
        phoneNumber1: '',
        phoneNumber2: ''
    },
    isActive: true,
    createdAt: '',
    createdBy: '',
    createdFullName: ''
}