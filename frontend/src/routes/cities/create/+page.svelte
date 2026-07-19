<script lang="ts">
  import { goto } from '$app/navigation';
  import { resolve } from '$app/paths';
  import { findCities, createCity } from '$lib/api/cities';
  import { savedCities } from '$lib/stores/savedCities.svelte';
  import type { FoundCity } from '$lib/api/types';

  let query = $state('');
  let results = $state<FoundCity[]>([]);
  let searching = $state(false);
  let searchError = $state<string | null>(null);

  let creatingKey = $state<string | null>(null);
  let createError = $state<string | null>(null);

  const keyOf = (c: FoundCity) => `${c.countryCode}:${c.name}`;

  async function runSearch(e: SubmitEvent) {
    e.preventDefault();
    const name = query.trim();
    if (!name) return;
    searching = true;
    searchError = null;
    createError = null;
    try {
      results = await findCities(name);
    } catch (err) {
      searchError = err instanceof Error ? err.message : String(err);
      results = [];
    } finally {
      searching = false;
    }
  }

  async function create(found: FoundCity) {
    creatingKey = keyOf(found);
    createError = null;
    try {
      const result = await createCity({
        name: found.name,
        countryCode: found.countryCode
      });
      if (!result.isSuccess) {
        createError = result.errorMessage ?? 'Create failed';
        return;
      }
      // Save it so it shows on the home page, then go there.
      savedCities.add({ cityId: result.cityId, name: found.name });
      await goto(resolve('/'));
    } catch (err) {
      createError = err instanceof Error ? err.message : String(err);
    } finally {
      creatingKey = null;
    }
  }
</script>

<div class="p-6">
  <a href={resolve('/')} class="text-sm text-blue-600 hover:underline">← Back</a>
  <h1 class="mb-4 mt-2 text-2xl font-bold">Create a city</h1>

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
  {/if}
  {#if createError}
    <p class="mb-4 text-red-600">{createError}</p>
  {/if}

  {#if results.length > 0}
    <ul class="divide-y rounded border">
      {#each results as result (keyOf(result))}
        <li class="flex items-center justify-between px-3 py-2">
          <span>{result.name}<span class="text-gray-500">, {result.country}</span></span>
          <button
            class="rounded border px-2 py-0.5 text-sm hover:bg-gray-100 disabled:opacity-50"
            disabled={creatingKey !== null}
            onclick={() => create(result)}
          >
            {creatingKey === keyOf(result) ? 'Creating…' : 'Create'}
          </button>
        </li>
      {/each}
    </ul>
  {:else if !searching}
    <p class="text-gray-500">Search for a city to create.</p>
  {/if}
</div>
