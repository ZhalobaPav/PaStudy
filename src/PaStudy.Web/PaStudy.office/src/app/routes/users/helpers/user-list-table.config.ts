import { SearchFilterComponent } from '../../../shared/components/table/filters/search/search.component';
import { TableConfig } from '../../../shared/components/table/models/table.models';

export const tableConfig: TableConfig = {
  headers: [
    {
      name: 'userName',
      label: 'Name',
      component: SearchFilterComponent,
      width: 120,
      componentInputs: {
        inputType: 'text',
      },
    },
  ],
};
