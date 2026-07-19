export interface CitySummary {
  cityId: string;
  name: string;
  countryName: string;
}

// Result of the external /cities/find lookup (not-yet-created cities).
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
  observedAt: string; // DateTimeOffset serializes as an ISO 8601 string
}

export interface CityDetails {
  cityId: string;
  countryCode: string;
  name: string;
  state: string;
  country: string;
  currencyCode: string | undefined;
  currentWeather: CurrentWeather;
}

export interface CreateCityRequest {
  name: string;
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

export interface DeleteCityResult {
  isSuccess: boolean;
  errorMessage: string | null;
}
