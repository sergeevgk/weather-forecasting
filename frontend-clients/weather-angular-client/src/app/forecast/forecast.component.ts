import { Component, Input } from '@angular/core';
import { IWeatherForecast, WeatherForecast } from './forecast.model';

@Component({
  selector: 'app-forecast',
  standalone: true,
  imports: [],
  templateUrl: './forecast.component.html',
  styleUrl: './forecast.component.css'
})
export class ForecastComponent {
  @Input()
  forecast: IWeatherForecast = new WeatherForecast('2024-01-01', 20, 40, 'cold');
  constructor() {
    
  }
}
