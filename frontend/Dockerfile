# ---- Build stage ----
FROM node:22-alpine AS build
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
# PUBLIC_API_BASE_URL is inlined into the client bundle at build time
# ($env/static/public), so it must be supplied here, not at runtime.
ARG PUBLIC_API_BASE_URL
ENV PUBLIC_API_BASE_URL=$PUBLIC_API_BASE_URL
RUN npm run build

# ---- Runtime stage ----
FROM node:22-alpine AS runtime
WORKDIR /app
ENV NODE_ENV=production

COPY --from=build /app/build ./build
COPY --from=build /app/package.json /app/package-lock.json ./
RUN npm ci --omit=dev

ENV PORT=3000
EXPOSE 3000
CMD ["node", "build"]
