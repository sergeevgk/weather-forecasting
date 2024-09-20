import { Component, Inject, Injectable, LOCALE_ID } from '@angular/core';
import { CommonModule, formatDate } from '@angular/common';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormsModule} from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { WeatherForecast, WeatherForecasts } from '../types/weatherForecast';
import { WeatherForecastResponse } from '../types/weatherForecastResponse';
import '../extensions/date.extensions'

@Injectable()
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, MatInputModule, MatSelectModule, MatFormFieldModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  MAX_FORECAST_DAYS: number = 5;
  cityName: string = "London";
  date: Date = new Date();
  title = 'Weather';
  forecasts: WeatherForecasts = [];
  cityList: string[] = [
    "London", "Paris", "Belgrade", "Vladivostok", "Washington", "New York", "Tokyo", "Dubai", "Rome", "Moscow", "Sydney"
  ];
  availableDateList: Date[] = [];

  constructor(private httpClient: HttpClient, @Inject(LOCALE_ID) private locale: string) {
    this.FetchForecasts();
    for (let i = 1; i < this.MAX_FORECAST_DAYS; i++){
      this.availableDateList.push(this.date.addDays(i));
    }
  }

  onSelectNewCity(){
    this.FetchForecasts();
  }

  onSelectNewDate(){
    this.FetchForecasts();
  }

  // /api/weatherforecast?city=${this.cityName}&date=${dateStr}`
  private FetchForecasts(){
    let dateStr = formatDate(this.date, 'yyyy-MM-dd', "en-US");
    this.httpClient.get<WeatherForecastResponse[]>(`/api/forecasts`).subscribe({
      next: result => this.forecasts = result[0].forecasts.map(
        f => new WeatherForecast(
          formatDate(f.localTime, 'yyyy-MM-dd HH:mm', this.locale), 
          f.weatherSummary.temperature.value, 
          +(f.weatherSummary.temperature.value * 9/5 + 32).toFixed(2),
        f.weatherSummary.description)),
      error: console.error
    });
  }
}
