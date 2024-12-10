import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';
import { FollowCommand, UserClient, UserDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-user-card',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './user-card.component.html',
  styleUrl: './user-card.component.css'
})
export class UserCardComponent {

  @Input()
  public gridClass: string = '';

  @Input()
  public columnClass: string = '';

  private userClient = inject(UserClient);

  public readonly users = inject(UserService).users;

  public follow(followedId: string) {
    const command = new FollowCommand();
    command.followedId = followedId;
    this.userClient.follow(command)
      .subscribe({
        next: response => {
          if (response.succeeded) {
            this.userClient.getUsers().subscribe({
              next: response => {
                this.users.update(prev => [...response]);
              }
            });
          }
        },
        error(err) {
          if (err instanceof HttpErrorResponse) {
            console.log(err.message);
          }
        },
      });
  }
}
