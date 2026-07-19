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

// A daily rollup of the 5-day / 3-hour forecast.
export interface DailyForecast {
  summary: string;
  condition: string;
  icon: string | null;
  date: string; // DateOnly serializes as "YYYY-MM-DD"
  minCelsius: number;
  maxCelsius: number;
  dayCelsius: number;
  humidity: number;
  windSpeed: number;
  precipitationChance: number; // 0–1
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
  dateEstablished: string | null; // "YYYY-MM-DD"
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
