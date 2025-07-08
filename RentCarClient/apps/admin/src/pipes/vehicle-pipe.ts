import { Pipe, PipeTransform } from '@angular/core';
import { VehicleModel } from '../models/vehicle.model';

@Pipe({
  name: 'vehicle'
})
export class VehiclePipe implements PipeTransform {

  transform(value: VehicleModel[], categoryName:string, fuelType:string, transmission:string): VehicleModel[] {
    if(categoryName === '' && fuelType === '' && transmission ==='') 
        return value;

    return value.filter(p =>
      p.categoryName.includes(categoryName)
      && p.fuelType.includes(fuelType)
      && p.transmission.includes(transmission)
    );
  }

}
