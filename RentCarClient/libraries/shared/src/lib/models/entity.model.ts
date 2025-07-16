export interface EntityModel{
    id: string;
    isActive: boolean;
    createdAt: string;
    createdBy: string;
    createdFullName: string;
    updatedAt?: string;
    updatedBy?: string;
    updatedFullName?: string;
}