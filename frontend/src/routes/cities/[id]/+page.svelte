<script lang="ts">
  import { resolve } from '$app/paths';
  import { goto, invalidateAll } from '$app/navigation';
  import { updateCity, deleteCity } from '$lib/api/cities';
  import { savedCities } from '$lib/stores/savedCities.svelte';
  import { weatherBackground } from '$lib/weather/background';
  import type { PageData } from './$types';

  let { data }: { data: PageData } = $props();

  const savedHere = $derived(savedCities.cities.some((c) => c.cityId === city.cityId));
  let deleteError = $state<string | null>(null);

  function toggleHome() {
    if (savedHere) savedCities.remove(city.cityId);
    else savedCities.add({ cityId: city.cityId, name: city.name });
  }

  // Delete the city entirely, drop it from the homepage, then go home.
  async function removeCity() {
    if (!confirm(`Delete ${city.name} permanently? This cannot be undone.`)) return;
    deleteError = null;
    try {
      const result = await deleteCity(city.cityId);
      if (!result.isSuccess) {
        deleteError = result.errorMessage ?? 'Delete failed';
        return;
      }
      savedCities.remove(city.cityId);
      await goto(resolve('/'));
    } catch (e) {
      deleteError = e instanceof Error ? e.message : String(e);
    }
  }

  // Edit updatable city facts (population, rating, established date)
  let editing = $state(false);
  let saving = $state(false);
  let saveError = $state<string | null>(null);

  let populationInput = $state('');
  let ratingInput = $state('');
  let dateInput = $state('');

  function startEdit() {
    populationInput = city.estimatedPopulation != null ? String(city.estimatedPopulation) : '';
    ratingInput = city.touristRating != null ? String(city.touristRating) : '';
    dateInput = city.dateEstablished ?? '';
    saveError = null;
    editing = true;
  }

  async function save(e: SubmitEvent) {
    e.preventDefault();
    saving = true;
    saveError = null;
    try {
      const result = await updateCity(city.cityId, {
        estimatedPopulation: populationInput === '' ? null : Number(populationInput),
        touristRating: ratingInput === '' ? null : Number(ratingInput),
        dateEstablished: dateInput === '' ? null : dateInput
      });
      if (!result.isSuccess) {
        saveError = result.errorMessage ?? 'Update failed';
        return;
      }
      await invalidateAll(); // re-run load so the facts reflect the saved values
      editing = false;
    } catch (err) {
      saveError = err instanceof Error ? err.message : String(err);
    } finally {
      saving = false;
    }
  }

  const city = $derived(data.city);
  const weather = $derived(city.currentWeather);
  const temperature = $derived(weather ? Math.round(weather.temperatureCelsius) : 0);
  const feelsLike = $derived(weather ? Math.round(weather.feelsLikeCelsius) : 0);

  // Current hour in the city's own timezone, from the country's UTC offset.
  // Might not be right in huge countries but since city = capital for now it should work
  const currentHour = $derived(
    new Date(Date.now() + city.utcOffsetSeconds * 1000).getUTCHours()
  );
  const backgroundUrl = $derived(weather ? weatherBackground(weather.condition, currentHour) : '');

  // When it was last refreshed/polled, would be a bit more relevant if I added caching and maybe a
  // specific reset button
  const observedAt = $derived(
    weather
      ? new Date(weather.observedAt).toLocaleTimeString(undefined, {
          hour: 'numeric',
          minute: '2-digit'
        })
      : ''
  );

  const population = $derived(city.estimatedPopulation?.toLocaleString() ?? '—');
  const touristRating = $derived(city.touristRating != null ? `${city.touristRating} ★` : '—');
  const established = $derived(
    city.dateEstablished
      ? new Date(city.dateEstablished).toLocaleDateString(undefined, {
          year: 'numeric',
          month: 'long',
          day: 'numeric'
        })
      : '—'
  );

  const forecast = $derived(city.forecast ?? []);

  function formatForecastDay(date: string) {
    return new Date(date).toLocaleDateString(undefined, {
      weekday: 'short',
      day: 'numeric',
      month: 'short'
    });
  }
</script>

<div class="mx-auto max-w-2xl p-6">
  <div class="flex items-center justify-between">
    <a href={resolve('/')} class="text-sm text-blue-600 hover:underline">← Back</a>
    <div class="flex gap-2">
      <button
        class="rounded border px-3 py-1 text-sm hover:bg-gray-100"
        onclick={toggleHome}
      >
        {savedHere ? 'Remove from home' : 'Add to home'}
      </button>
      <button
        class="rounded border px-3 py-1 text-sm text-red-600 hover:bg-red-50"
        onclick={removeCity}
      >
        Delete
      </button>
    </div>
  </div>

  {#if deleteError}
    <p class="mt-2 text-sm text-red-600">{deleteError}</p>
  {/if}

  {#if weather}
    <!-- Weather hero -->
    <div class="mt-2 overflow-hidden rounded-lg shadow">
      <div class="{backgroundUrl} bg-cover bg-no-repeat px-6 py-10 text-white">
        <p class="text-lg font-semibold">{weather.summary}</p>
        <p class="text-6xl font-bold">{temperature}°C</p>
        <div class="mt-4">
          <h1 class="text-2xl font-bold">{city.name}</h1>
          <p class="text-sm opacity-90">{city.country}</p>
        </div>
      </div>
    </div>

    <!-- Weather stats -->
    <div class="mt-4 grid grid-cols-3 gap-3">
      <div class="rounded-lg border p-4 text-center">
        <p class="text-xs uppercase tracking-wide text-gray-500">Feels like</p>
        <p class="mt-1 text-xl font-semibold">{feelsLike}°C</p>
      </div>
      <div class="rounded-lg border p-4 text-center">
        <p class="text-xs uppercase tracking-wide text-gray-500">Humidity</p>
        <p class="mt-1 text-xl font-semibold">{weather.humidity}%</p>
      </div>
      <div class="rounded-lg border p-4 text-center">
        <p class="text-xs uppercase tracking-wide text-gray-500">Wind</p>
        <p class="mt-1 text-xl font-semibold">{weather.windSpeed} m/s</p>
      </div>
    </div>
  {:else}
    <!-- Incase detail doesn't return current weather -->
    <div class="mt-2">
      <h1 class="text-2xl font-bold">{city.name}</h1>
      <p class="text-sm text-gray-500">{city.country}</p>
    </div>
  {/if}

  <!-- City facts -->
  <div class="mt-6 flex items-center justify-between">
    <h2 class="text-lg font-semibold">About {city.name}</h2>
    {#if !editing}
      <button
        class="rounded border px-3 py-1 text-sm hover:bg-gray-100"
        onclick={startEdit}
      >
        Edit
      </button>
    {/if}
  </div>

  {#if editing}
    <form class="mt-2 space-y-3 rounded-lg border p-4" onsubmit={save}>
      <label class="block">
        <span class="text-xs uppercase tracking-wide text-gray-500">Population</span>
        <input
          type="number"
          min="0"
          step="1"
          bind:value={populationInput}
          class="mt-1 w-full rounded border px-3 py-1"
        />
      </label>
      <label class="block">
        <span class="text-xs uppercase tracking-wide text-gray-500">Tourist rating</span>
        <input
          type="number"
          min="0"
          max="5"
          step="0.1"
          bind:value={ratingInput}
          class="mt-1 w-full rounded border px-3 py-1"
        />
      </label>
      <label class="block">
        <span class="text-xs uppercase tracking-wide text-gray-500">Established</span>
        <input
          type="date"
          bind:value={dateInput}
          class="mt-1 w-full rounded border px-3 py-1"
        />
      </label>

      {#if saveError}
        <p class="text-sm text-red-600">{saveError}</p>
      {/if}

      <div class="flex gap-2">
        <button
          type="submit"
          disabled={saving}
          class="rounded border bg-blue-600 px-3 py-1 text-sm text-white hover:bg-blue-700 disabled:opacity-50"
        >
          {saving ? 'Saving…' : 'Save'}
        </button>
        <button
          type="button"
          disabled={saving}
          class="rounded border px-3 py-1 text-sm hover:bg-gray-100 disabled:opacity-50"
          onclick={() => (editing = false)}
        >
          Cancel
        </button>
      </div>
    </form>
  {/if}

  <div class="mt-2 grid grid-cols-2 gap-3 sm:grid-cols-4">
    <div class="rounded-lg border p-4 text-center">
      <p class="text-xs uppercase tracking-wide text-gray-500">Population</p>
      <p class="mt-1 whitespace-nowrap text-base font-semibold tabular-nums">{population}</p>
    </div>
    <div class="rounded-lg border p-4 text-center">
      <p class="text-xs uppercase tracking-wide text-gray-500">Tourist rating</p>
      <p class="mt-1 text-xl font-semibold">{touristRating}</p>
    </div>
    <div class="rounded-lg border p-4 text-center">
      <p class="text-xs uppercase tracking-wide text-gray-500">Established</p>
      <p class="mt-1 text-sm font-semibold">{established}</p>
    </div>
    <div class="rounded-lg border p-4 text-center">
      <p class="text-xs uppercase tracking-wide text-gray-500">Currency</p>
      <p class="mt-1 text-xl font-semibold">{city.currencyCode ?? '—'}</p>
    </div>
  </div>

  {#if weather}
    <p class="mt-4 text-sm text-gray-500">Updated {observedAt}</p>
  {/if}

  <!-- 5-day forecast -->
  {#if forecast.length}
    <h2 class="mt-6 text-lg font-semibold">5-day forecast</h2>
    <div class="mt-2 flex gap-3 overflow-x-auto pb-2">
      {#each forecast as day (day.date)}
        <div class="flex w-24 shrink-0 flex-col rounded-lg border p-3 text-center">
          <p class="text-xs font-medium text-gray-500">{formatForecastDay(day.date)}</p>
          <p class="mt-1 text-xs capitalize text-gray-600">{day.summary}</p>
          <div class="mt-auto pt-2">
            {#if day.precipitationChance > 0}
              <p class="text-xs text-blue-500">{Math.round(day.precipitationChance * 100)}%</p>
            {/if}
            <p class="text-sm font-semibold">
              {Math.round(day.maxCelsius)}°<span class="text-gray-400"
                >/{Math.round(day.minCelsius)}°C</span
              >
            </p>
          </div>
        </div>
      {/each}
    </div>
  {/if}
</div>
