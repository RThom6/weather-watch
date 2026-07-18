# weather-watch
Seiko takehome test

### Notes - 16/07/2026

- Overall following Clean architecture. Split it into Api, Infrastructure, and Application
- Infra has my httpClient, Dtos, and some config for OpenWeather (url and api key)
- Application has features. So in this case weather but since that's all this app is it won't get more than that probably. It also has some Domain objects. 
- I coulda made Domain but in the interest of keeping this smaller, this was easier and less bulky.
- Api defines my actual endpoints


### Notes - 17/07/2026

- City != any city, needs to be capital city for restcountries
- City object needs those fields but create doesn't necessarily
- Minimal info db needs to build request -> City name, City Id -> allows search to work.


- just found capitals on the country object and it has lat long. this info is huge news
- Before that I was trying to think of how to tie things in and it really just wasn't working out in my brain

- Using entity framework for first time seems pretty nifty

- Capabilities so far:
  - Create -> creates a city from some basic info. This I can get if I list of the cities on the FE using the countryApi and search feature.
  - Get Details -> Gets some basic weather details and city details, can flesh out with front end
  - Search -> Searches for cities by name, maybe make name not a guid so that searching for cityId works better? Not fully sure on the functionality here tbh
