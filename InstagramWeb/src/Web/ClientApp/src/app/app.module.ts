import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap/modal';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { TodoComponent } from './todo/todo.component';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserModule } from './core/modules/user/user.module';
import { UserCardComponent } from "./core/components/cards/user-card/user-card.component";
import { UserPostModule } from './core/modules/user-post/user-post.module';
import { refreshTokenInterceptor } from 'src/api-authorization/refresh-token.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    TodoComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'user', loadChildren: () => import('./core/modules/user/user.module').then(m => m.UserModule) },
      { path: 'user-post', loadChildren: () => import('./core/modules/user-post/user-post.module').then(m => m.UserPostModule) },

    ]),
    BrowserAnimationsModule,
    ModalModule.forRoot(),
    UserModule,
    UserCardComponent,
    ReactiveFormsModule,
    FormsModule,
    UserPostModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    provideHttpClient(withFetch(), withInterceptors([refreshTokenInterceptor]))
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
