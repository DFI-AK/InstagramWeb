import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountClient, LoginRequest } from 'src/app/web-api-client';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private accountClient = inject(AccountClient)

  public loginForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }


  loginUser() {
    console.log(this.loginForm)
    if (this.loginForm.valid) {
      const loginRequest = new LoginRequest()
      loginRequest.email = this.loginForm.get('email')?.value
      loginRequest.password = this.loginForm.get('password')?.value
      this.accountClient.postApiAccountLogin(null, null, loginRequest)
        .subscribe({
          next: token => {
            console.log(token.accessToken);
          }
        })
    }
  }

}
