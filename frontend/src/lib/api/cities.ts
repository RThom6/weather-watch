import type {
  CityDetails,
  CitySummary,
  CreateCityRequest,
  CreateCityResult,
  DeleteCityResult,
  FindCitiesResult,
  FoundCity,
  UpdateCityRequest,
  UpdateCityResult
} from './types';
import { PUBLIC_API_BASE_URL } from '$env/static/public';

const API_BASE = PUBLIC_API_BASE_URL;

export async function searchCities(
  name: string,
  page = 0,
  pageSize = 50,
  fetchFn: typeof fetch = fetch
): Promise<CitySummary[]> {
  const res = await fetchFn(
    `${API_BASE}/cities/search?name=${encodeURIComponent(name)}&page=${page}&pageSize=${pageSize}`
  );
  if (!res.ok) throw new Error(`Search failed: ${res.status}`);
  return res.json();
}

export async function findCities(
  name: string,
  fetchFn: typeof fetch = fetch
): Promise<FoundCity[]> {
  const res = await fetchFn(`${API_BASE}/cities/find?name=${encodeURIComponent(name)}`);
  if (!res.ok) throw new Error(`Find failed: ${res.status}`);
  const result: FindCitiesResult = await res.json();
  return result.cities;
}

export async function getCityDetailsPreview(
  cityId: number,
  fetchFn: typeof fetch = fetch
): Promise<CityDetails> {
  // no-store so a refresh always pulls fresh weather rather than a cached response
  const res = await fetchFn(`${API_BASE}/cities/${cityId}/details/preview`, { cache: 'no-store' });
  if (!res.ok) throw new Error(`GetCityDetailsPreview failed: ${res.status}`);
  return res.json();
}

// Full details for the city page facts + current weather + the 5 day forecast
export async function getCityDetails(
  cityId: number,
  fetchFn: typeof fetch = fetch
): Promise<CityDetails> {
  // no-store so a refresh always pulls fresh weather rather than a cached response
  const res = await fetchFn(`${API_BASE}/cities/${cityId}/details`, { cache: 'no-store' });
  if (!res.ok) throw new Error(`GetCityDetails failed: ${res.status}`);
  return res.json();
}

export async function createCity(
  request: CreateCityRequest,
  fetchFn: typeof fetch = fetch
): Promise<CreateCityResult> {
  const res = await fetchFn(`${API_BASE}/cities/create`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request)
  });
  if (!res.ok) throw new Error(`CreateCity failed: ${res.status}`);
  return res.json();
}

export async function deleteCity(
  cityId: number,
  fetchFn: typeof fetch = fetch
): Promise<DeleteCityResult> {
  const res = await fetchFn(`${API_BASE}/cities/${cityId}`, { method: 'DELETE' });
  if (!res.ok) throw new Error(`DeleteCity failed: ${res.status}`);
  return res.json();
}

export async function updateCity(
  cityId: number,
  request: UpdateCityRequest,
  fetchFn: typeof fetch = fetch
): Promise<UpdateCityResult> {
  const res = await fetchFn(`${API_BASE}/cities/${cityId}/update`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request)
  });
  if (!res.ok) throw new Error(`UpdateCity failed: ${res.status}`);
  return res.json();
}
