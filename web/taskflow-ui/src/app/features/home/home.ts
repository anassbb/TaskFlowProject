import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  template: `
    <section class="card">
      <h2>Bienvenue sur TaskFlow</h2>
      <p>Le squelette est en place : API .NET 10, Aspire, Angular 22 zoneless.</p>
      <p>Prochaine étape : le module <strong>Projects</strong> (domaine, endpoints, puis cet écran).</p>
    </section>
  `,
  styles: `
    .card { background: #fff; border: 1px solid #e2e2e6; border-radius: 12px; padding: 20px 24px; }
    h2 { margin-top: 0; }
  `,
})
export class Home {}
