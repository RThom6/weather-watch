import { browser } from '$app/environment';

export interface SavedCity {
  cityId: string;
  name: string;
}

const STORAGE_KEY = 'saved-cities';

function read(): SavedCity[] {
  if (!browser) return [];
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? (JSON.parse(raw) as SavedCity[]) : [];
  } catch {
    return [];
  }
}

function write(list: SavedCity[]) {
  if (browser) localStorage.setItem(STORAGE_KEY, JSON.stringify(list));
}

class SavedCitiesStore {
  cities = $state<SavedCity[]>(read());

  add(city: SavedCity) {
    if (this.cities.some((c) => c.cityId === city.cityId)) return;
    this.cities.push(city);
    write(this.cities);
  }

  remove(cityId: string) {
    this.cities = this.cities.filter((c) => c.cityId !== cityId);
    write(this.cities);
  }
}

export const savedCities = new SavedCitiesStore();
