import { TableConfig } from '../../../../../shared/components/table/models/table.models';

export const tableConfig: TableConfig = {
  headers: [
    {
      name: 'studentFullName',
      label: 'Студент',
      component: null,
      width: 300,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'submittedAt',
      label: 'Дата здачі',
      component: null,
      width: 300,
      componentInputs: {
        inputType: 'date',
      },
    },
    {
      name: 'Status',
      label: 'Статус',
      component: null,
      width: 400,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'Grade',
      label: 'Оцінка',
      component: null,
      width: 300,
      componentInputs: {
        inputType: 'number',
      },
    },
  ],
};
