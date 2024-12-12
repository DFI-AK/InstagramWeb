import { Component, OnInit, inject } from '@angular/core';
import { UserClient } from '../web-api-client';
import { HttpErrorResponse } from '@angular/common/http';
import { UserService } from '../core/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  ngOnInit(): void {
    this.getUsers();
  }

  private userService = inject(UserClient);
  public users = inject(UserService).users;

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
