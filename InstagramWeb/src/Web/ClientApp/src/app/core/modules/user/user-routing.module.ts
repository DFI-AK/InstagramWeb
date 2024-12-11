import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatWindowComponent } from './pages/chat-window/chat-window.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
  { path: "message", component: ChatWindowComponent },
  { path:"login", component:LoginComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
