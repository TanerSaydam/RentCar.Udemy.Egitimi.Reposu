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