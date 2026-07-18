export interface CitySummary {
  cityId: string;
  name: string;
  countryName: string;
}

export interface CurrentWeather {
  summary: string;
  temperatureCelsius: number;
  feelsLikeCelsius: number;
  humidity: number;
  windSpeed: number;
  observedAt: string; // DateTimeOffset serializes as an ISO 8601 string
}

export interface CityDetails {
  cityId: string;
  countryCode: string;
  name: string;
  state: string;
  countryName: string;
  currencyCode: string | undefined;
  currentWeather: CurrentWeather;
}

export interface CreateCityRequest {
  name: string;
  state: string;
  country: string;
  countryCode: string;
}

export interface CreateCityResult {
  isSuccess: boolean;
  errorMessage: string | null;
  cityId: string;
}

export interface UpdateCityRequest {
  touristRating: number;
  dateEstablished: Date;
  estimatedPopulation: number;
}

export interface UpdateCityResult {
  isSuccess: boolean;
  errorMessage: string | null;
  cityId: string;
}