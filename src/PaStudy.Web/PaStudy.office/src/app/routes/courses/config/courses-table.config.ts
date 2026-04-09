import { SearchFilterComponent } from '../../../shared/components/table/filters/search/search.component';
import { TableConfig } from '../../../shared/components/table/models/table.models';

export const tableConfig: TableConfig = {
  headers: [
    {
      name: 'searchTerm',
      label: 'Назва курсу',
      component: SearchFilterComponent,
      width: 300,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'description',
      label: 'Опис курсу',
      component: null,
      width: 650,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'Category',
      label: 'Категорія',
      component: null,
      width: 200,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'Teachers',
      label: 'Викладачі',
      component: null,
      width: 300,
      componentInputs: {
        inputType: 'text',
      },
    },
  ],
};
