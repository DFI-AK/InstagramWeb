import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
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
  public users: Array<UserDto> = [];

  @Input()
  public gridClass: string = '';

  @Input()
  public columnClass: string = '';

  private userService = inject(UserClient);

  public follow(followedId: string) {
    const command = new FollowCommand();
    command.followedId = followedId;
    this.userService.follow(command)
      .subscribe({
        next: response => {
          console.log(response);
        },
        error(err) {
          if (err instanceof HttpErrorResponse) {
            console.log(err.message);
          }
        },
      });
  }
}
