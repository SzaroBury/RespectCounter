const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/api",
    "/register",
    "/login"
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware({
        target: 'http://localhost:5292',
        secure: false,
        changeOrigin: true,
        logLevel: 'debug', // Pokaże szczegóły dotyczące przekierowań w konsoli
    });

    app.use(context, appProxy);
};
