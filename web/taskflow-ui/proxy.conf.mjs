// Redirige /api/* du serveur de dev Angular vers l'API .NET.
// Quand l'app est lancée par l'AppHost Aspire, l'adresse de l'API est injectée en variable d'environnement
// (WithReference(api)). Hors Aspire, on retombe sur l'adresse locale par défaut de l'API.
const apiUrl =
  process.env['API_HTTPS'] ??
  process.env['services__api__https__0'] ??
  process.env['API_HTTP'] ??
  process.env['services__api__http__0'] ??
  'https://localhost:7209';

export default {
  '/api': {
    target: apiUrl,
    secure: false, // certificat de dev ASP.NET auto-signé
    changeOrigin: true,
    logLevel: 'info',
  },
};
