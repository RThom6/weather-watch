<script lang="ts">
  import CityCard from '$lib/components/CityCard.svelte';
  import { resolve } from '$app/paths';
  import { savedCities } from '$lib/stores/savedCities.svelte';
  import { getCityDetails, searchCities } from '$lib/api/cities';
  import type { CityDetails, CitySummary } from '$lib/api/types';

  let cities = $state<CityDetails[]>([]);
  let loading = $state(false);
  let error = $state<string | null>(null);

  // Search state
  let query = $state('');
  let results = $state<CitySummary[]>([]);
  let searching = $state(false);
  let searchError = $state<string | null>(null);

  async function runSearch(e: SubmitEvent) {
    e.preventDefault();
    const name = query.trim();
    if (!name) return;
    searching = true;
    searchError = null;
    try {
      results = await searchCities(name);
    } catch (err) {
      searchError = err instanceof Error ? err.message : String(err);
      results = [];
    } finally {
      searching = false;
    }
  }

  function save(result: CitySummary) {
    savedCities.add({ cityId: result.cityId, name: result.name });
  }

  const isSaved = (cityId: string) => savedCities.cities.some((c) => c.cityId === cityId);

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
    Promise.all(saved.map((c) => getCityDetails(c.cityId)))
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
    <a
      href={resolve('/cities/create')}
      class="rounded border px-3 py-1 hover:bg-gray-100"
    >
      Create
    </a>
  </div>

  <form class="mb-4 flex gap-2" onsubmit={runSearch}>
    <input
      type="text"
      bind:value={query}
      placeholder="Search for a city…"
      class="w-full max-w-sm rounded border px-3 py-1"
    />
    <button
      type="submit"
      disabled={searching}
      class="rounded border px-3 py-1 hover:bg-gray-100 disabled:opacity-50"
    >
      {searching ? 'Searching…' : 'Search'}
    </button>
  </form>

  {#if searchError}
    <p class="mb-4 text-red-600">Search failed: {searchError}</p>
  {:else if results.length > 0}
    <ul class="mb-6 divide-y rounded border">
      {#each results as result (result.cityId)}
        <li class="flex items-center justify-between px-3 py-2">
          <span>{result.name}<span class="text-gray-500">, {result.countryName}</span></span>
          <button
            class="rounded border px-2 py-0.5 text-sm hover:bg-gray-100 disabled:opacity-50"
            disabled={isSaved(result.cityId)}
            onclick={() => save(result)}
          >
            {isSaved(result.cityId) ? 'Saved' : 'Save'}
          </button>
        </li>
      {/each}
    </ul>
  {/if}

  {#if error}
    <p class="text-red-600">Failed to load cities: {error}</p>
  {:else if loading && cities.length === 0}
    <p>Loading…</p>
  {:else if cities.length === 0}
    <p class="text-gray-500">No saved cities yet.</p>
  {:else}
    <div class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      {#each cities as city (city.cityId)}
        <CityCard {city} />
      {/each}
    </div>
  {/if}
</div>
