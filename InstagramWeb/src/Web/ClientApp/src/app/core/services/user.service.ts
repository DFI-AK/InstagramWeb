import { HttpErrorResponse } from '@angular/common/http';
import { effect, Inject, inject, Injectable, OnDestroy, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AccountClient, LoginRequest, UserClient, UserDto, UserPost, UserProfileVm } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root'
})
export class UserService implements OnDestroy {
  ngOnDestroy(): void {
    this.loggedInUserEffect.destroy()
  }

  constructor(@Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl
  }

  public readonly users = signal<UserDto[]>([]);
  //public readonly usersPost = signal<UserPost[]>([]);
  private baseUrl = ''
  public readonly loggedInUser = signal<UserProfileVm | null>(null);
  public readonly router = inject(Router)
  private readonly accountClient = inject(AccountClient);
  private readonly userClient = inject(UserClient);

  private loggedInUserEffect = effect(() => {
    const user = this.loggedInUser()
    const token = localStorage.getItem('token')
    const loginUrl = this.baseUrl + 'Identity/Account/Login'
     if (!user && !token) {
      window.location.replace(loginUrl)
    }

    if (!user && token) {
      this.userClient.getUserInfo()
        .subscribe({
          next: response => {
            this.loggedInUser.set(response)
          },
          error: (err) => {
            window.location.replace(loginUrl)
          }
        })
    }
  })

  public logout(): void {
    const tokenStore = localStorage.getItem('token');
    if (!tokenStore)
      throw new Error("Token is not available");

    localStorage.removeItem('token');
  }

  public login = (email: string, password: string) => {
    const loginRequest = new LoginRequest();
    loginRequest.email = email;
    loginRequest.password = password;
    this.accountClient.postApiAccountLogin(null, null, loginRequest)
      .subscribe({
        next: response => {
          if (response) {
            debugger;
            localStorage.setItem('token', JSON.stringify(response));
            this.userClient.getUserInfo()
              .subscribe({
                next: userProfile => {
                  this.loggedInUser.set(userProfile);
                },
                error: err => {
                  if (err instanceof HttpErrorResponse) {
                    console.log(err.message);
                  }
                }
              });
          }
        },
        error: err => {
          if (err instanceof HttpErrorResponse) {
            console.log(err.message);
          }
        }
      });
  };
}
