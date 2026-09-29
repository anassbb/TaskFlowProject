import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { App } from './app';

describe('App', () => {
  let http: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    }).compileComponents();

    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('affiche "Connectée" quand /api/info répond', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();

    http.expectOne('/api/info').flush({
      name: 'TaskFlow API',
      version: '1.0.0',
      environment: 'Development',
      serverTime: '2026-09-29T12:00:00Z',
    });
    await fixture.whenStable();

    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('app-status-badge')?.textContent).toContain('Connectée');
    expect(el.textContent).toContain('TaskFlow API');
  });

  it('affiche "Injoignable" quand l\'API est en erreur', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();

    http.expectOne('/api/info').flush('KO', { status: 503, statusText: 'Service Unavailable' });
    await fixture.whenStable();

    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('app-status-badge')?.textContent).toContain('Injoignable');
  });
});
