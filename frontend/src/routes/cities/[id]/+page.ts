import type { PageLoad } from './$types';
import { getCityDetails } from '$lib/api/cities';

export const load: PageLoad = async ({ params, fetch }) => {
  const city = await getCityDetails(Number(params.id), fetch);
  return { city };
};
