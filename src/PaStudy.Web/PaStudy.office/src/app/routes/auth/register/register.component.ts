import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { customPasswordValidator } from '../shared/validators/password.validator';
import { confirmPasswordValidator } from '../shared/validators/confirmPassword.validator';
import { AuthService } from '../auth.service';
import { UserRole } from '../../../shared/enums/userRole';
import { catchError, take, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { GroupsService } from '../../groups/groups.service';
import { IGroup } from '../../../shared/models/group';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  private router = inject(Router);
  ngOnInit(): void {
    this.initSubscriptions();
  }
  initSubscriptions() {
    this.groupService
      .getGroups()
      .pipe(take(1))
      .subscribe((groups) => {
        console.log(groups);
        this.groups = groups;
      });
  }

  private readonly fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);
  private groupService = inject(GroupsService);

  public groups: IGroup[] = [];

  registerForm = this.fb.nonNullable.group(
    {
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      middleName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, customPasswordValidator()]],
      confirmPassword: ['', [Validators.required, customPasswordValidator()]],
      dateOfBirth: [new Date(), Validators.required],
      phoneNumber: ['', Validators.required],
      role: [UserRole.Student, Validators.required],
      displayName: ['', Validators.required],
      groupId: [null as number | null],
    },
    { validators: [confirmPasswordValidator] },
  );

  public submit(): void {
    const formValue = this.registerForm.getRawValue();

    const payload = {
      ...formValue,
      role: Number(formValue.role),
      groupId: formValue.groupId !== null ? Number(formValue.groupId) : null,
    };
    this.authService
      .register(payload)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        tap((response) => {
          this.router.navigate(['']);
        }),
      )
      .subscribe();
  }

  getFormErrors(): any {
    const errors: any = {};
    Object.keys(this.registerForm.controls).forEach((key) => {
      const controlErrors = this.registerForm.get(key)?.errors;
      if (controlErrors) {
        errors[key] = controlErrors;
      }
    });
    return errors;
  }
}
