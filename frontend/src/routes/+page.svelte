<script lang="ts">
  import CityCard from '$lib/components/CityCard.svelte';
  import { resolve } from '$app/paths';
  import { savedCities } from '$lib/stores/savedCities.svelte';
  import { getCityDetailsPreview, deleteCity } from '$lib/api/cities';
  import type { CityDetails } from '$lib/api/types';

  let cities = $state<CityDetails[]>([]);
  let loading = $state(false);
  let error = $state<string | null>(null);

  // Remove from the homepage only (leaves the city in the database).
  function removeFromHomepage(cityId: number) {
    savedCities.remove(cityId);
  }

  // Delete the city entirely, then drop it from the homepage.
  async function deleteEntirely(city: CityDetails) {
    if (!confirm(`Delete ${city.name} permanently? This cannot be undone.`)) return;
    try {
      const result = await deleteCity(city.cityId);
      if (!result.isSuccess) {
        error = result.errorMessage ?? 'Delete failed';
        return;
      }
      savedCities.remove(city.cityId);
    } catch (e) {
      error = e instanceof Error ? e.message : String(e);
    }
  }

  // Whenever the saved list changes, refetch details for each saved city.
  // Runs client-side only (effects don't run during SSR), so localStorage is safe to read.
  $effect(() => {
    const saved = savedCities.cities;
    if (saved.length === 0) {
      cities = [];
      return;
    }
    loading = true;
    error = null;
    Promise.all(saved.map((c) => getCityDetailsPreview(c.cityId)))
      .then((results) => {
        cities = results;
      })
      .catch((e) => {
        error = e instanceof Error ? e.message : String(e);
      })
      .finally(() => {
        loading = false;
      });
  });
</script>

<div class="p-6">
  <div class="mb-4 flex items-center justify-between">
    <h1 class="text-2xl font-bold">My cities</h1>
    <div class="flex gap-2">
      <a href={resolve('/cities/search')} class="rounded border px-3 py-1 hover:bg-gray-100">
          Search
      </a>
      <a href={resolve('/cities/create')} class="rounded border px-3 py-1 hover:bg-gray-100">
        Create
      </a>
    </div>
  </div>

  {#if error}
    <p class="text-red-600">Failed to load cities: {error}</p>
  {:else if loading && cities.length === 0}
    <p>Loading…</p>
  {:else if cities.length === 0}
    <p class="text-gray-500">No cities on your homepage yet.</p>
  {:else}
    <div class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      {#each cities as city (city.cityId)}
        <CityCard
          {city}
          onRemove={() => removeFromHomepage(city.cityId)}
          onDelete={() => deleteEntirely(city)}
        />
      {/each}
    </div>
  {/if}
</div>
