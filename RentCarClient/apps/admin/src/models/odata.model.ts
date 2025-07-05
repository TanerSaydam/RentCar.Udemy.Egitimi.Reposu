export interface ODataModel<T>{
    value: T[];
    ['@odata.count']: number
}