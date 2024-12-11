import { Component, inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private userService = inject(UserService);
  private readonly fb = inject(FormBuilder)

  public loginForm = this.fb.group({
    email:['',[]]
  })
}
