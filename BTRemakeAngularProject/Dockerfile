# Stage 1: Build de l'application
FROM node:18-alpine AS build

WORKDIR /app

# Copier les fichiers de dépendances
COPY package*.json ./

# Installer TOUTES les dépendances (y compris devDependencies pour le build)
RUN npm ci

# Copier le code source
COPY . .

# Build de production (crée le dossier dist/)
RUN npm run build --prod

# Stage 2: Servir avec Nginx
FROM nginx:1.23-alpine

# Copier les fichiers buildés depuis le stage précédent
COPY --from=build /app/dist/btremake-angular-project/browser/ /usr/share/nginx/html/

# Configuration Nginx pour SPA
RUN rm /etc/nginx/conf.d/default.conf
COPY default.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
