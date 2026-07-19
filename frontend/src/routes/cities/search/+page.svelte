<script lang="ts">
  import { resolve } from '$app/paths';
  import { searchCities } from '$lib/api/cities';
  import { savedCities } from '$lib/stores/savedCities.svelte';
  import type { CitySummary } from '$lib/api/types';

  const PAGE_SIZE = 5;

  let cities = $state<CitySummary[]>([]);
  let query = $state('');
  let page = $state(0);
  let loading = $state(false);
  let loadingMore = $state(false);
  let error = $state<string | null>(null);
  let hasMore = $state(false);

  // Guards against out-of-order responses when typing quickly.
  let requestId = 0;

  async function load(reset: boolean) {
    const id = ++requestId;
    const nextPage = reset ? 0 : page + 1;
    if (reset) loading = true;
    else loadingMore = true;
    error = null;
    try {
      const results = await searchCities(query.trim(), nextPage, PAGE_SIZE);
      if (id !== requestId) return; // a newer request was made
      page = nextPage;
      cities = reset ? results : [...cities, ...results];
      hasMore = results.length === PAGE_SIZE;
    } catch (e) {
      if (id === requestId) error = e instanceof Error ? e.message : String(e);
    } finally {
      if (id === requestId) {
        loading = false;
        loadingMore = false;
      }
    }
  }

  // Debounced search on the backend whenever the query changes
  $effect(() => {
    void query;
    const timer = setTimeout(() => load(true), 300);
    return () => clearTimeout(timer);
  });

  function add(city: CitySummary) {
    savedCities.add({ cityId: city.cityId, name: city.name });
  }

  function remove(city: CitySummary) {
    savedCities.remove(city.cityId);
  }

  function toggleAdded(city:CitySummary) {
    if(isSaved(city.cityId)) { remove(city); } else {add(city);}
  }

  const isSaved = (cityId: number) => savedCities.cities.some((c) => c.cityId === cityId);
</script>

<div class="mx-auto max-w-2xl p-6">
  <a href={resolve('/')} class="text-sm text-blue-600 hover:underline">← Back</a>
  <h1 class="mb-4 mt-2 text-2xl font-bold">Search cities</h1>

  <input
    type="text"
    bind:value={query}
    placeholder="Search by name…"
    class="mb-4 w-full max-w-sm rounded border px-3 py-1"
  />

  {#if error}
    <p class="text-red-600">Failed to load cities: {error}</p>
  {:else if loading}
    <p>Loading…</p>
  {:else if cities.length === 0}
    <p class="text-gray-500">
      {query.trim() ? `No cities match “${query}”.` : 'No cities exist yet. Try creating one first.'}
    </p>
  {:else}
    <ul class="divide-y rounded border">
      {#each cities as city (city.cityId)}
        <li class="flex items-center justify-between px-3 py-2">
          <a
            href={resolve('/cities/[id]', { id: String(city.cityId) })}
            class="flex-1 hover:underline"
          >
            {city.name}<span class="text-gray-500">, {city.countryName}</span>
          </a>
          <button
            class="rounded border px-2 py-0.5 text-sm hover:bg-gray-100 disabled:opacity-50"
            onclick={() => toggleAdded(city)}
          >
            {isSaved(city.cityId) ? 'Remove from home' : 'Add to home'}
          </button>
        </li>
      {/each}
    </ul>

    {#if hasMore}
      <button
        class="mt-4 rounded border px-3 py-1 hover:bg-gray-100 disabled:opacity-50"
        disabled={loadingMore}
        onclick={() => load(false)}
      >
        {loadingMore ? 'Loading…' : 'Load more'}
      </button>
    {/if}
  {/if}
</div>
