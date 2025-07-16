import { EntityModel } from "./entity.model";

export interface CategoryModel extends EntityModel {
  name: string;
  createdFullName: string;
  updatedFullName: string;
}

export const initialCategoryModel: CategoryModel = {
  id: '',
  name: '',
  createdAt: '',
  createdBy: '',
  updatedAt: '',
  updatedBy: '',
  isActive: true,
  createdFullName: '',
  updatedFullName: '',
};