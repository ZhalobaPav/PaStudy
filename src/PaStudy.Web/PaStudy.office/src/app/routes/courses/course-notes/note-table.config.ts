import { SearchFilterComponent } from '../../../shared/components/table/filters/search/search.component';
import { TableConfig } from '../../../shared/components/table/models/table.models';

export const tableConfig: TableConfig = {
  headers: [
    {
      name: 'searchTerm',
      label: 'Назва завдання',
      component: SearchFilterComponent,
      width: 300,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'grade',
      label: 'Оцінка',
      component: null,
      width: 50,
      componentInputs: {
        inputType: 'number',
      },
    },
    {
      name: 'maxPoints',
      label: 'Максимальна оцінка',
      component: null,
      width: 80,
      componentInputs: {
        inputType: 'number',
      },
    },
    {
      name: 'percantage',
      label: 'Відсоток',
      component: null,
      width: 50,
      componentInputs: {
        inputType: 'number',
      },
    },
  ],
};
