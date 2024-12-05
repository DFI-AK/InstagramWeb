import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { SignalrService } from 'src/app/core/services/signalr.service';

@Component({
  selector: 'app-chat-window',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './chat-window.component.html',
  styleUrl: './chat-window.component.css'
})
export class ChatWindowComponent implements OnInit {
  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe({
      next: param => {
        const { userId } = param;
        this.userId = userId;
      }
    });
  }
  private activatedRoute = inject(ActivatedRoute);
  private signalr = inject(SignalrService);
  public chats = inject(SignalrService).chats;
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
