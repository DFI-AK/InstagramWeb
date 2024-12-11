import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AccessTokenResponse, AccountClient, BaseUserDto, LoginRequest, UserClient, UserDto, UserProfileVm } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public readonly users = signal<UserDto[]>([]);

  public readonly loggedInUser = signal<UserProfileVm | null>(null);

  private readonly accountClient = inject(AccountClient);
  private readonly userClient = inject(UserClient);

  public logout(): Observable<void> {
    const tokenStore = localStorage.getItem('token');
    if (!tokenStore)
      throw new Error("Token is not available");

    const token = JSON.parse(tokenStore) as AccessTokenResponse;
    return new Observable<void>()
      .pipe(tap(() => {
        if (token) {
          localStorage.removeItem('token');
          this.loggedInUser.set(null);
        }
      }));
  }

  public login = (email: string, password: string) => {
    const loginRequest = new LoginRequest();
    loginRequest.email = email;
    loginRequest.password = password;
    this.accountClient.postApiAccountLogin(null, null, loginRequest)
      .subscribe({
        next: response => {
          if (response) {
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
