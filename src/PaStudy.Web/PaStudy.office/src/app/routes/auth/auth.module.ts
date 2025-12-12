import { NgModule } from '@angular/core';
import { RegisterComponent } from './register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './auth.service';
import { AuthRoutingModule } from './auth.routing.module';
import { JsonPipe } from '@angular/common';

@NgModule({
  declarations: [RegisterComponent],
  imports: [ReactiveFormsModule, AuthRoutingModule, JsonPipe],
  providers: [AuthService],
})
export class AuthModule {}
