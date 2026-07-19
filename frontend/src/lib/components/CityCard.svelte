<script module lang="ts">
  // A single shared document listener for all cards' outside-click handling,
  // instead of one listener per card. Handlers register/unregister per instance.
  const outsideHandlers = new SvelteSet<(e: MouseEvent) => void>();
  let listenerAttached = false;

  function ensureListener() {
    if (listenerAttached || typeof document === 'undefined') return;
    document.addEventListener('click', (e) => outsideHandlers.forEach((h) => h(e)), true);
    listenerAttached = true;
  }
</script>

<script lang="ts">
  import { resolve } from '$app/paths';
  import type { CityDetails } from '$lib/api/types';
  import { weatherBackground } from '$lib/weather/background';
	import { SvelteSet } from 'svelte/reactivity';

  let {
    city,
    onRemove,
    onDelete
  }: {
    city: CityDetails;
    onRemove?: () => void;
    onDelete?: () => void;
  } = $props();

  let menuOpen = $state(false);

  // Close the menu when clicking anywhere outside it, via the shared listener.
  function closeOnOutside(node: HTMLElement) {
    ensureListener();
    const handle = (event: MouseEvent) => {
      if (menuOpen && !node.contains(event.target as Node)) menuOpen = false;
    };
    outsideHandlers.add(handle);
    return {
      destroy() {
        outsideHandlers.delete(handle);
      }
    };
  }

  // `summary` is the human-readable text ("broken clouds"); `condition` is the
  // OpenWeather "main" bucket ("Clouds") used to pick the background.
  const weatherState = $derived(city.currentWeather?.summary ?? '');
  const condition = $derived(city.currentWeather?.condition ?? '');
  const degreesCelcius = $derived(Math.round(city.currentWeather?.temperatureCelsius ?? 0));

  // Day/night from the viewer's local hour. The city's own timezone isn't on
  // CityDetails yet, so this uses the browser's clock for now.
  const currentHour = new Date().getHours();

  const backgroundUrl = $derived(weatherBackground(condition, currentHour));
</script>

<div
  class="relative m-4 w-full overflow-hidden rounded-lg shadow transition-shadow hover:shadow-lg md:w-auto md:min-w-87.5"
>
  <!-- Actions menu (siblings of the link so they aren't nested in an <a>) -->
  {#if onRemove || onDelete}
    <div class="absolute right-2 top-2 z-10" use:closeOnOutside>
      <button
        type="button"
        aria-label="City actions"
        aria-haspopup="menu"
        aria-expanded={menuOpen}
        class="flex h-7 w-7 items-center justify-center rounded-full bg-white/80 text-gray-700 hover:bg-white"
        onclick={() => (menuOpen = !menuOpen)}
      >
        ⋯
      </button>

      {#if menuOpen}
        <div
          role="menu"
          class="absolute right-0 mt-1 w-40 overflow-hidden rounded-md border bg-white text-sm shadow-lg"
        >
          {#if onRemove}
            <button
              type="button"
              role="menuitem"
              class="block w-full px-3 py-2 text-left text-gray-700 hover:bg-gray-100"
              onclick={() => {
                menuOpen = false;
                onRemove?.();
              }}
            >
              Remove from homepage
            </button>
          {/if}
          {#if onDelete}
            <button
              type="button"
              role="menuitem"
              class="block w-full px-3 py-2 text-left text-red-600 hover:bg-red-50"
              onclick={() => {
                menuOpen = false;
                onDelete?.();
              }}
            >
              Delete city
            </button>
          {/if}
        </div>
      {/if}
    </div>
  {/if}

  <a href={resolve('/cities/[id]', { id: String(city.cityId) })} class="block">
    <!-- Weather background header -->
    <div class="{backgroundUrl} bg-cover bg-no-repeat px-6 py-8 text-white">
      <p class="text-lg font-semibold">{weatherState}</p>
      <p class="text-5xl font-bold">{degreesCelcius}°C</p>
    </div>

    <div class="border-t p-4">
      <h2 class="text-lg font-semibold">{city.name}</h2>
      <p class="text-sm text-gray-500">{city.country}</p>
    </div>
  </a>
</div>
