import { SearchFilterComponent } from '../../../shared/components/table/filters/search/search.component';
import { SelectFilterComponent } from '../../../shared/components/table/filters/select-filter/select-filter.component';
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
    {
      name: 'userRole',
      label: 'Role',
      component: SelectFilterComponent,
      width: 190,
      componentInputs: {
        options: [
          { label: 'Student', value: 'Student' },
          { label: 'Teacher', value: 'Teacher' },
        ],
      },
    },
  ],
};
