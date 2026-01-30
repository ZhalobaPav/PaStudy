import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import {
  HTTP_INTERCEPTORS,
  HttpClient,
  HttpClientModule,
  provideHttpClient,
  withInterceptors,
} from '@angular/common/http';
import { AuthInterceptor } from './core/interceptors/auth.intercept';
import { AuthModule } from './routes/auth/auth.module';
import { tokenInterceptor } from './core/interceptors/token.intereceptor';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    AuthModule,
    HttpClientModule,
  ],
  bootstrap: [AppComponent],
  providers: [
    provideAnimationsAsync('noop'),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    provideHttpClient(withInterceptors([tokenInterceptor])),
  ],
})
export class AppModule {}
