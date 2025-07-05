import { EntityModel } from "./entity.model";

export interface RoleModel extends EntityModel{
    name: string;
}

export const initialRole: RoleModel = {
    id: '',
    name: '',
    isActive: true,
    createdAt: '',
    createdBy: '',
    createdFullName: ''
}