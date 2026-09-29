import { Component, computed, input } from '@angular/core';

export type Status = 'loading' | 'ok' | 'error';

/** Petit badge réutilisable : un composant "shared" ne connaît aucun service métier, seulement ses inputs. */
@Component({
  selector: 'app-status-badge',
  template: `<span class="badge" [class]="status()">{{ label() }}</span>`,
  styles: `
    .badge { display: inline-block; padding: 2px 10px; border-radius: 999px; font-size: 0.8rem; font-weight: 600; }
    .loading { background: #eef0ff; color: #4f46e5; }
    .ok { background: #e9f8f1; color: #047857; }
    .error { background: #fdecec; color: #b42318; }
  `,
})
export class StatusBadge {
  readonly status = input.required<Status>();
  readonly text = input<string>();

  protected readonly label = computed(() => {
    const labels: Record<Status, string> = { loading: 'Connexion…', ok: 'Connectée', error: 'Injoignable' };
    return this.text() ?? labels[this.status()];
  });
}
