// Lance "ng serve" sur le port fourni par Aspire (variable PORT), 4200 par défaut.
// Passer par Node évite la différence $PORT (Linux/macOS) / %PORT% (Windows) dans package.json.
import { spawn } from 'node:child_process';

const port = process.env['PORT'] ?? '4200';
const child = spawn('npx', ['ng', 'serve', '--port', port, '--host', 'localhost'], {
  stdio: 'inherit',
  shell: true,
});

child.on('exit', (code) => process.exit(code ?? 0));
for (const signal of ['SIGINT', 'SIGTERM']) {
  process.on(signal, () => child.kill(signal));
}
