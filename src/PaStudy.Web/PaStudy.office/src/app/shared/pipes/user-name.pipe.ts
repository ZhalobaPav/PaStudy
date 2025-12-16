import { Pipe, PipeTransform } from '@angular/core';
import { User } from '../models/user';

@Pipe({
  name: 'userName',
  standalone: true,
})
export class UserNamePipe implements PipeTransform {
  transform(user: User): string | undefined {
    if (!user) return;

    return `${user.lastName} ${user.firstName} ${user.middleName || ''}`;
  }
}
