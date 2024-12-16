import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllUsersPostComponent } from './pages/all-users-post/all-users-post.component';

const routes: Routes = [
   { path: "usersPost", component: AllUsersPostComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserPostRoutingModule { }
