import { EntityModel } from "./entity.model";

export interface BranchModel extends EntityModel{
    name: string;
    address: AddressModel;
}

export interface AddressModel{
    city: string;
    district: string;
    fullAddress:string;
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
        fullAddress: '',
        email: '',
        phoneNumber1: '',
        phoneNumber2: ''
    },
    isActive: true,
    createdAt: '',
    createdBy: '',
    createdFullName: ''
}