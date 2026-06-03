import { SearchFilterComponent } from '../../../shared/components/table/filters/search/search.component';
import { TableConfig } from '../../../shared/components/table/models/table.models';

export const gradebookTableConfig: TableConfig = {
  headers: [
    {
      name: 'searchTerm',
      label: 'Студент',
      component: SearchFilterComponent,
      width: 200,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'groupName',
      label: 'Група',
      component: null,
      width: 100,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'progress',
      label: 'Прогрес виконання',
      component: null,
      width: 100,
      componentInputs: {
        inputType: 'number',
      },
    },
    {
      name: 'enrolledAt',
      label: 'Дата запису',
      component: null,
      width: 120,
      componentInputs: {
        inputType: 'date',
      },
    },
    {
      name: 'finalGrade',
      label: 'Підсумкова оцінка',
      component: null,
      width: 130,
      componentInputs: {
        inputType: 'number',
      },
    },
  ],
};
