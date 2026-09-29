import { Injectable } from '@angular/core';
import { httpResource } from '@angular/common/http';

/** Réponse de GET /api/info (voir TaskFlow.Api/Infrastructure/InfoEndpoint.cs). */
export interface ApiInfo {
  name: string;
  version: string;
  environment: string;
  serverTime: string;
}

@Injectable({ providedIn: 'root' })
export class ApiInfoService {
  /**
   * httpResource : requête HTTP exposée sous forme de signals
   * (value(), isLoading(), error()), sans subscribe ni gestion manuelle de l'état.
   */
  readonly info = httpResource<ApiInfo>(() => '/api/info');
}
