# Stage 1: build the Angular app. Node is only needed here.
FROM node:24-alpine AS build
WORKDIR /src

# Install dependencies
COPY package.json package-lock.json ./
RUN npm ci

COPY . ./
RUN npm run build

# Stage 2: serve the built files.
FROM nginx:1.31-alpine
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf
# The application builder writes to dist/<project>/browser.
COPY --from=build /src/dist/duck-debug-app/browser /usr/share/nginx/html
EXPOSE 80
