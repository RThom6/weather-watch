import { browser } from '$app/environment';

export interface SavedCity {
  cityId: number;
  name: string;
}

const STORAGE_KEY = 'saved-cities';

// TODO: buggy behaviour if data gets removed outside of the FE
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

  remove(cityId: number) {
    this.cities = this.cities.filter((c) => c.cityId !== cityId);
    write(this.cities);
  }
}

export const savedCities = new SavedCitiesStore();
