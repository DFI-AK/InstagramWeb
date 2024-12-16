import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatWindowComponent } from './pages/chat-window/chat-window.component';
import { LoginComponent } from './login/login.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { UserProfileComponent } from './user-profile/user-profile.component';

const routes: Routes = [
  { path: "message", component: ChatWindowComponent },
  { path: "login", component: LoginComponent },
  { path: "signUp", component: SignUpComponent },
  { path: "user/profile", component: UserProfileComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
