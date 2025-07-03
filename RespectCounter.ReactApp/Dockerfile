FROM node:22.16.0 AS build
WORKDIR /app
COPY RespectCounter.ReactApp/package*.json ./
RUN npm install
COPY RespectCounter.ReactApp/. .
RUN npm run build

FROM nginx:alpine
RUN apk add --no-cache bash
COPY --from=build /app/dist /usr/share/nginx/html
COPY RespectCounter.ReactApp/default.conf /etc/nginx/conf.d/default.conf
COPY wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh
EXPOSE 80