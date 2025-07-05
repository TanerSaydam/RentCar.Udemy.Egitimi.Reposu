import { EntityModel } from "./entity.model";

export interface RoleModel extends EntityModel{
    name: string;
    permissionCount: number;
    permissions: string[];
}

export const initialRole: RoleModel = {
    id: '',
    name: '',
    permissionCount: 0,
    permissions: [],
    isActive: true,
    createdAt: '',
    createdBy: '',
    createdFullName: ''
}