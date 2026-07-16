# weather-watch
Seiko takehome test starter. Just messing about learning about some stuff for if I get the takehome test.

### Notes - 16/07/2026

- Overall following Clean architecture. Split it into Api, Infrastructure, and Application
- Infra has my httpClient, Dtos, and some config for OpenWeather (url and api key)
- Application has features. So in this case weather but since that's all this app is it won't get more than that probably. It also has some Domain objects. 
- I coulda made Domain but in the interest of keeping this smaller, this was easier and less bulky.
- Api defines my actual endpoints