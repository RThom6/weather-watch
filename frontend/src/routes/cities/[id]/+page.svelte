<script lang="ts">
  import type { PageData } from './$types';

  let { data }: { data: PageData } = $props();
</script>

<h1>{data.city.name}</h1>
<p>{data.city.state}, {data.city.country}</p>
<pre>{JSON.stringify(data.city.currentWeather, null, 2)}</pre>