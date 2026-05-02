import { SearchFilterComponent } from '../../../shared/components/table/filters/search/search.component';
import { SelectFilterComponent } from '../../../shared/components/table/filters/select-filter/select-filter.component';
import { TableConfig } from '../../../shared/components/table/models/table.models';

export const tableConfig: TableConfig = {
  headers: [
    {
      name: 'userName',
      label: 'Name',
      component: SearchFilterComponent,
      width: 200,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'dateOfBirth',
      label: 'Birth date',
      component: null,
      width: 130,
      componentInputs: {
        inputType: 'date',
      },
    },
    {
      name: 'email',
      label: 'Email',
      component: null,
      width: 190,
      componentInputs: {
        inputType: 'text',
      },
    },
    {
      name: 'groupNumber',
      label: 'Group number',
      component: null,
      width: 190,
      componentInputs: {
        inputType: 'number',
      },
    },
  ],
};
