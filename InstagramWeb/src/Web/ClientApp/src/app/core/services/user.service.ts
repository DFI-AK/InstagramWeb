import { Injectable, signal } from '@angular/core';
import { UserDto } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public readonly users = signal<UserDto[]>([])
  
}
