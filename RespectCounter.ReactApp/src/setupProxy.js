import { createProxyMiddleware } from 'http-proxy-middleware';

const context = [
    "/api",
    "/register",
    "/login"
];

export default function (app) {
    const appProxy = createProxyMiddleware({
        target: 'http://localhost:5292',
        secure: false,
        changeOrigin: true,
        logLevel: 'debug',
    });

    app.use(context, appProxy);
};
