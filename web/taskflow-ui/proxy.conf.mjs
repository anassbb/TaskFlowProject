// Redirige /api/* du serveur de dev Angular vers la Gateway (le front ne parle jamais directement aux microservices).
// Lancé par l'AppHost Aspire, l'adresse de la Gateway est injectée en variable d'environnement
// (WithReference(gateway)). Hors Aspire, on retombe sur l'adresse locale par défaut de la Gateway.
const gatewayUrl =
  process.env['GATEWAY_HTTPS'] ??
  process.env['services__gateway__https__0'] ??
  process.env['GATEWAY_HTTP'] ??
  process.env['services__gateway__http__0'] ??
  'https://localhost:7288';

export default {
  '/api': {
    target: gatewayUrl,
    secure: false, // certificat de dev ASP.NET auto-signé
    changeOrigin: true,
    logLevel: 'info',
  },
};
