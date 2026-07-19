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
  fetchFn: typeof fetch = fetch
): Promise<CitySummary[]> {
  const res = await fetchFn(`${API_BASE}/cities/search?name=${encodeURIComponent(name)}`);
  if (!res.ok) throw new Error(`Search failed: ${res.status}`);
  return res.json();
}

// External lookup for cities that can be created (not yet in the DB).
export async function findCities(
  name: string,
  fetchFn: typeof fetch = fetch
): Promise<FoundCity[]> {
  const res = await fetchFn(`${API_BASE}/cities/find?name=${encodeURIComponent(name)}`);
  if (!res.ok) throw new Error(`Find failed: ${res.status}`);
  const result: FindCitiesResult = await res.json();
  return result.cities;
}

export async function getCityDetails(
  cityId: string,
  fetchFn: typeof fetch = fetch
): Promise<CityDetails> {
  const res = await fetchFn(`${API_BASE}/cities/${cityId}/details`);
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
  cityId: string,
  fetchFn: typeof fetch = fetch
): Promise<DeleteCityResult> {
  const res = await fetchFn(`${API_BASE}/cities/${cityId}/delete`);
  if (!res.ok) throw new Error(`DeleteCity failed: ${res.status}`);
  return res.json();
}

export async function updateCity(
  cityId: string,
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
