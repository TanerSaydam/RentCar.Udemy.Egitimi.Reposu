import { EntityModel } from "./entity.model";

export interface ExtraModel extends EntityModel {
  name: string;
  description: string;
  price: number;
}

export const initialExtraModel: ExtraModel = {
  id: '',
  name: '',
  description: '',
  price: 0,
  createdAt: '',
  createdBy: '',
  updatedAt: '',
  updatedBy: '',
  isActive: true,
  createdFullName: '',
  updatedFullName: '',
};