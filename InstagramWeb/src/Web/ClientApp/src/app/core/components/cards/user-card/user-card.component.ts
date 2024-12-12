import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, inject, Input, ViewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';
import { CreatePostCommand, FollowCommand, UnfollowCommand, UserClient, UserDto, UserPostClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-user-card',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './user-card.component.html',
  styleUrl: './user-card.component.css'
})
export class UserCardComponent {
  @ViewChild('postContentInput') postContentInput!: ElementRef<HTMLTextAreaElement>;

  @Input()
  public gridClass: string = '';

  @Input()
  public columnClass: string = '';

  private userClient = inject(UserClient);
   

  private userService = inject(UserClient);
  private postService = inject(UserPostClient);

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

  public unfollowUser(followedId: string) {
    const command = new UnfollowCommand();
    command.followedId = followedId;

    this.userClient.unfollow(command)
      .subscribe({
        next: response => {
          if (response.succeeded) {
            this.userClient.getUsers().subscribe({
              next: user => {
                this.users.update(prev => [...user]);
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

  public createPost(content: string) {
    const command = new CreatePostCommand()
    command.content = content
    command.imageUrls = []

     if (content.trim() === '') {
        alert('Post content cannot be empty.');
        return;
    } 
    this.postService.post(command)
      .subscribe({
        next: response => {
          console.log('Post created successfully:', response);
           // Clear the textarea after success
        this.postContentInput.nativeElement.value = '';
        },
        error: (err) => {
          console.error('Error creating post:', err);
        },
      })
}
}
