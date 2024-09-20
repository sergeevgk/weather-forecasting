export interface IWeatherForecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

export class WeatherForecast implements IWeatherForecast{
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
    constructor(date: string, temperatureC: number, temperatureF: number, summary: string) {
        this.date = date;
        this.temperatureC = temperatureC;
        this.temperatureF = temperatureF;
        this.summary = summary;
    }
}

export type WeatherForecasts = IWeatherForecast[];