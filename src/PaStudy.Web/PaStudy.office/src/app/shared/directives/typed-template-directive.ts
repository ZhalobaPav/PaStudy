import { Directive, Input, TemplateRef } from '@angular/core';

@Directive({
  selector: '[typedTemplate]',
  exportAs: 'typedTemplate',
  standalone: true,
})
export class TypedTemplate<T extends Record<string, any>> {
  @Input('typedTemplate') context!: T;

  constructor(public templateRef: TemplateRef<T>) {}

  static ngTemplateContextGuard<T extends Record<string, any>>(
    directive: TypedTemplate<T>,
    context: unknown
  ): context is T {
    return true;
  }
}
