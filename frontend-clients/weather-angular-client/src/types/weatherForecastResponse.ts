export class WeatherForecastResponse {
    place: Place;
    forecasts: Forecast[];

    constructor(place: Place, forecasts: Forecast[]) {
        this.place = place;
        this.forecasts = forecasts;
    }
}

class Place {
    city: string;
    countryCode: string;
    longitude: number;
    latitude: number;

    constructor(city: string, countryCode: string, longitude: number, latitude: number) {
        this.city = city;
        this.countryCode = countryCode;
        this.longitude = longitude;
        this.latitude = latitude;
    }
}

class Forecast {
    localTime: string; // ISO 8601 string
    utcTime: string; // ISO 8601 string
    weatherSummary: WeatherSummary;
    wind: Wind;

    constructor(localTime: string, utcTime: string, weatherSummary: WeatherSummary, wind: Wind) {
        this.localTime = localTime;
        this.utcTime = utcTime;
        this.weatherSummary = weatherSummary;
        this.wind = wind;
    }
}

class WeatherSummary {
    main: string;
    description: string;
    temperature: Temperature;
    pressure: number;
    humidity: number;
    cloudyPercentage: number;
    rainVolume3Hour: number;
    rainVolume1Hour: number;
    snowVolume3Hour: number;
    snowVolume1Hour: number;

    constructor(
        main: string,
        description: string,
        temperature: Temperature,
        pressure: number,
        humidity: number,
        cloudyPercentage: number,
        rainVolume3Hour: number,
        rainVolume1Hour: number,
        snowVolume3Hour: number,
        snowVolume1Hour: number
    ) {
        this.main = main;
        this.description = description;
        this.temperature = temperature;
        this.pressure = pressure;
        this.humidity = humidity;
        this.cloudyPercentage = cloudyPercentage;
        this.rainVolume3Hour = rainVolume3Hour;
        this.rainVolume1Hour = rainVolume1Hour;
        this.snowVolume3Hour = snowVolume3Hour;
        this.snowVolume1Hour = snowVolume1Hour;
    }
}

class Temperature {
    value: number;
    feelsLike: number;

    constructor(value: number, feelsLike: number) {
        this.value = value;
        this.feelsLike = feelsLike;
    }
}

class Wind {
    speed: number;
    deg: number;
    gust: number;

    constructor(speed: number, deg: number, gust: number) {
        this.speed = speed;
        this.deg = deg;
        this.gust = gust;
    }
}