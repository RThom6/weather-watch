export interface CitySummary {
  cityId: number;
  name: string;
  countryName: string;
}

export interface FoundCity {
  name: string;
  country: string;
  state: string;
  countryCode: string;
}

export interface FindCitiesResult {
  cities: FoundCity[];
}

export interface CurrentWeather {
  summary: string;
  condition: string;
  temperatureCelsius: number;
  feelsLikeCelsius: number;
  humidity: number;
  windSpeed: number;
  observedAt: string;
}

export interface DailyForecast {
  summary: string;
  condition: string;
  icon: string | null;
  date: string;
  minCelsius: number;
  maxCelsius: number;
  dayCelsius: number;
  humidity: number;
  windSpeed: number;
  precipitationChance: number;
}

export interface CityDetails {
  cityId: number;
  countryCode: string;
  name: string;
  country: string;
  currencyCode: string | undefined;
  estimatedPopulation: number | null;
  touristRating: number | null;
  dateEstablished: string | null;
  currentWeather?: CurrentWeather;
  forecast: DailyForecast[];
  utcOffsetSeconds: number; // shift from UTC for the city's timezone
}

export interface CreateCityRequest {
  name: string;
  countryCode: string;
}

export interface CreateCityResult {
  isSuccess: boolean;
  errorMessage: string | null;
  cityId: number;
}

export interface UpdateCityRequest {
  touristRating: number | null;
  dateEstablished: string | null;
  estimatedPopulation: number | null;
}

export interface UpdateCityResult {
  isSuccess: boolean;
  errorMessage: string | null;
  cityId: number;
}

export interface DeleteCityResult {
  isSuccess: boolean;
  errorMessage: string | null;
}
