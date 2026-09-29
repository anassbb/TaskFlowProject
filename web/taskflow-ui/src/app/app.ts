import { Component, computed, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiInfoService } from './core/api/api-info';
import { Status, StatusBadge } from './shared/ui/status-badge';

@Component({
  imports: [RouterOutlet, StatusBadge],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
  private readonly api = inject(ApiInfoService);

  protected readonly info = this.api.info;

  protected readonly apiStatus = computed<Status>(() => {
    if (this.info.isLoading()) return 'loading';
    return this.info.error() ? 'error' : 'ok';
  });
}
