import { Component, OnInit, inject } from '@angular/core';
import { UserClient, UserPostClient } from '../web-api-client';
import { HttpErrorResponse } from '@angular/common/http';
import { UserService } from '../core/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  ngOnInit(): void {
    this.getUsers();
    this.getUsersPosts()
  }

  private userService = inject(UserClient);
  private userPostService = inject(UserPostClient);
  public users = inject(UserService).users;

  public usersPost: any[] = []; // Property to hold users posts data

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



  public getUsersPosts(){
    this.userPostService.getAllUsersPosts().subscribe({
          next: response => {
            if (response.length > 0) {
              // this.users.set(response);
              this.usersPost = response; // Save the response in usersPost
              console.log("getuserPost", response)
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
