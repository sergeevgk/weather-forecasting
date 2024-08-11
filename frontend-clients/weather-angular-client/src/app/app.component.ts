import { Component, Inject, Injectable, LOCALE_ID } from '@angular/core';
import { CommonModule, formatDate } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { WeatherForecast, WeatherForecasts } from '../types/weatherForecast';
import { WeatherForecastResponse } from '../types/weatherForecastResponse';

@Injectable()
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Weather';
  forecasts: WeatherForecasts = [];

  constructor(private http: HttpClient, @Inject(LOCALE_ID) private locale: string) {
    let currentDateStr = new Date();
    let tomorrowDate = currentDateStr.setDate(currentDateStr.getDate() + 1);
    let tomorrowDateStr = formatDate(tomorrowDate, 'yyyy-MM-dd', "en-US");
    http.get<WeatherForecastResponse>(`https://localhost:7208/weatherforecast?city=London&date=${tomorrowDateStr}`).subscribe({
      next: result => this.forecasts = result.forecasts.map(
        f => new WeatherForecast(
          formatDate(f.localTime, 'yyyy-MM-dd HH:mm', locale), 
          f.weatherSummary.temperature.value, 
          +(f.weatherSummary.temperature.value * 9/5 + 32).toFixed(2),
        f.weatherSummary.description)),
      error: console.error
    });
  }
}
