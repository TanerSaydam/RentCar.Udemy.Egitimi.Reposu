export interface Result<T>{
    data?: T;
    errorMessages?: string[];
    isSuccessful: boolean;
    statusCode: number;
}