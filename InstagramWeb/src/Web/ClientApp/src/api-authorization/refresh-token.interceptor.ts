import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { UserService } from 'src/app/core/services/user.service';
import { AccessTokenResponse, AccountClient, RefreshRequest } from 'src/app/web-api-client';

export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const storedToken = localStorage.getItem('token');
  const userService = inject(UserService);
  const accountClient = inject(AccountClient);
  if (!storedToken) return next(req);

  const token = JSON.parse(storedToken) as AccessTokenResponse;

  const cloneReq = token
    ? req.clone({
      setHeaders: {
        "Authorization": `Bearer ${token.accessToken}`
      }
    })
    : req;

  return next(cloneReq)
    .pipe(catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        const refreshRequest = new RefreshRequest();
        refreshRequest.refreshToken = token.refreshToken;
        return accountClient.postApiAccountRefresh(refreshRequest)
          .pipe(
            switchMap(refrehToken => {
              const retryReq = req.clone({
                setHeaders: {
                  "Authorization": `Bearer ${refrehToken.accessToken}`
                }
              });
              return next(retryReq);
            }),
            catchError(err => {
              userService.logout();
              return throwError(() => err);
            })
          );
      }
      return throwError(() => error);
    }));
};
