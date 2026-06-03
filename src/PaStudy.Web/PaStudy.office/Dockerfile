# Етап 1: Збірка Angular додатка
FROM node:20 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build -- --configuration=production

# Етап 2: Роздача через Nginx
FROM nginx:alpine
# Копіюємо зібраний Angular (перевір точний шлях у папці dist твого проєкту)
COPY --from=build /app/dist/pa-study.office/browser /usr/share/nginx/html
# Копіюємо кастомний конфіг Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80