import { Component, OnInit, effect, inject, signal } from '@angular/core';
import { UserClient, UserDto } from '../web-api-client';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  ngOnInit(): void {
    this.getUsers();
  }

  private userEffect = effect(() => {
    console.log(this.users());
  });

  private userService = inject(UserClient);
  public users = signal<Array<UserDto>>([]);

  public getUsers() {
    this.userService.getUsers()
      .subscribe({
        next: response => {
          if (response.length > 0) {
            this.users.set(response);
          }
        },
        error: error => {
          if (error instanceof HttpErrorResponse) {
            console.log(error.message);
          }
        }
      });
  }
}
