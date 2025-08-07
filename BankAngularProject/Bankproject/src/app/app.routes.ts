import { Routes } from '@angular/router';
import { NotFoundcomponentComponent } from './not-foundcomponent/not-foundcomponent.component';
 import { RegisterComponent } from './register/register.component';
import { Login2Component } from './login2/login2.component';
import { HomeComponent } from './home/home.component';
import { CreateExpensesComponent } from './create-expenses/create-expenses.component';
import { EditExpensesComponent } from './edit-expenses/edit-expenses.component';
import { WelcomePageComponent } from './welcome-page/welcome-page.component';
 
export const routes: Routes = [

      { path: '', redirectTo: 'register', pathMatch: 'full' },
      { path: 'register', component: RegisterComponent },
      { path:'login', component: Login2Component },
      { path:'home', component: HomeComponent },
      { path:'create', component: CreateExpensesComponent },
      { path: 'edit/:id', component: EditExpensesComponent },
      { path: 'welcome', component: WelcomePageComponent },
     { path: "**", component: NotFoundcomponentComponent },



];
