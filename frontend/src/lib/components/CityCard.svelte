<script lang="ts">
  import { resolve } from '$app/paths';
  import type { CityDetails } from '$lib/api/types';

  let { city }: { city: CityDetails } = $props();

  // `summary` is the human-readable text ("broken clouds"); `condition` is the
  // OpenWeather "main" bucket ("Clouds") used to pick the background.
  const weatherState = $derived(city.currentWeather.summary);
  const condition = $derived(city.currentWeather.condition);
  const degreesCelcius = $derived(Math.round(city.currentWeather.temperatureCelsius));

  // Day/night from the viewer's local hour. The city's own timezone isn't on
  // CityDetails yet, so this uses the browser's clock for now.
  const currentHour = new Date().getHours();

  // Create and decide on background URL. It supports every possible weather condition in Open Weather API. You can add or delete options. There are 2 variants for "Clear" and "Clouds" for night times.
  const backgroundUrl = $derived.by(() => {
    const isCloudy =
      condition == 'Clouds' ||
      condition == 'Mist' ||
      condition == 'Smoke' ||
      condition == 'Haze' ||
      condition == 'Dust' ||
      condition == 'Fog' ||
      condition == 'Sand' ||
      condition == 'Ash' ||
      condition == 'Squall';
    const isDaytime = 7 < currentHour && currentHour < 20;

    if (condition == 'Clear' && isDaytime) {
      return "bg-[url('/weatherBg/sunny-animated.svg')]";
    } else if (condition == 'Clear') {
      return "bg-[url('/weatherBg/night-animated.svg')]";
    } else if (isCloudy && isDaytime) {
      return "bg-[url('/weatherBg/cloudy-animated.svg')]";
    } else if (isCloudy) {
      return "bg-[url('/weatherBg/cloudy-night-animated.svg')]";
    } else if (condition == 'Rain' || condition == 'Tornado' || condition == 'Drizzle') {
      return "bg-[url('/weatherBg/rainy-animated.svg')]";
    } else if (condition == 'Thunderstorm') {
      return "bg-[url('/weatherBg/thunderstorm-animated.svg')]";
    } else if (condition == 'Snow') {
      return "bg-[url('/weatherBg/snowy-animated.svg')]";
    }
    return "bg-[url('/weatherBg/sunny-animated.svg')]";
  });
</script>

<a
  href={resolve('/cities/[id]', { id: city.cityId })}
  class="m-4 block w-full overflow-hidden rounded-lg shadow transition-shadow hover:shadow-lg md:w-auto md:min-w-87.5"
>
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
