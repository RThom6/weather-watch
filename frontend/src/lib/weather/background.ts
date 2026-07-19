// Maps an OpenWeather "main" condition bucket to a background image class.
// Shared by CityCard and the city detail page so the visuals stay in sync.
//
// Supports every possible weather condition in the OpenWeather API. You can add
// or delete options. There are 2 variants for "Clear" and "Clouds" for night.
export function weatherBackground(condition: string, hour: number): string {
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
  const isDaytime = 7 < hour && hour < 20;

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
}
