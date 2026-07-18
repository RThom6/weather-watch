import type { CityDetails, CitySummary, CreateCityRequest, CreateCityResult } from './types';
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
  const params = new URLSearchParams(request);
  const res = await fetchFn(`${API_BASE}/cities/create?${params}`, { method: 'POST' });
  if (!res.ok) throw new Error(`CreateCity failed: ${res.status}`);
  return res.json();
}