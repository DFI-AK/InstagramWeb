import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalrService } from 'src/app/core/services/signalr.service';

@Component({
  selector: 'app-chat-window',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule,CommonModule],
  templateUrl: './chat-window.component.html',
  styleUrl: './chat-window.component.css'
})
export class ChatWindowComponent implements OnInit {
  public chats = inject(SignalrService).chats;
  //userId: string | null = null;
  userName: string | null = null;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.userName = navigation?.extras.state?.['userName'] || 'Unknown User';
  }
  
  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe({
      next: param => {
        const { userId } = param;
        this.userId = userId;
        console.log('Chats messages:', this.chats()?.messages);
      }
    });
  }
  private activatedRoute = inject(ActivatedRoute);
  private signalr = inject(SignalrService);

  private fb = inject(FormBuilder);

  public messageForm = this.fb.group({
    message: ['', [Validators.required]]
  });
  public userId: string = '';

  get message() {
    return this.messageForm.get('message');
  }

  public sendMessage() {
    const message = this.message.value;
    if (message && this.userId) {
      this.signalr.sendMessage(this.userId, message);
      this.messageForm.reset();
    }
  }

}
