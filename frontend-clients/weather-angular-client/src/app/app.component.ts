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

@Injectable()
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, MatInputModule, MatSelectModule, MatFormFieldModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  cityName: string = "London";
  title = 'Weather';
  forecasts: WeatherForecasts = [];
  cityList: string[] = [
    "London", "Paris", "Belgrade", "Vladivostok", "Washington", "New York", "Tokyo", "Dubai", "Rome", "Moscow", "Sydney"
  ];

  constructor(private httpClient: HttpClient, @Inject(LOCALE_ID) private locale: string) {
    this.FetchForecasts();
  }

  onSelectNewCity(){
    this.FetchForecasts();
  }

  private FetchForecasts(){
    let currentDateStr = new Date();
    let tomorrowDate = currentDateStr.setDate(currentDateStr.getDate() + 1);
    let tomorrowDateStr = formatDate(tomorrowDate, 'yyyy-MM-dd', "en-US");
    this.httpClient.get<WeatherForecastResponse>(`/api/weatherforecast?city=${this.cityName}&date=${tomorrowDateStr}`).subscribe({
      next: result => this.forecasts = result.forecasts.map(
        f => new WeatherForecast(
          formatDate(f.localTime, 'yyyy-MM-dd HH:mm', this.locale), 
          f.weatherSummary.temperature.value, 
          +(f.weatherSummary.temperature.value * 9/5 + 32).toFixed(2),
        f.weatherSummary.description)),
      error: console.error
    });
  }
}
