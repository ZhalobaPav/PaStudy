import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, share } from 'rxjs';

export interface MenuTag {
  color: string; // background color
  value: string;
}

export interface MenuPermissions {
  only?: string | string[];
  except?: string | string[];
}

export interface MenuChildrenItem {
  route: string;
  name: string;
  type: 'link' | 'sub' | 'extLink' | 'extTabLink';
  children?: MenuChildrenItem[];
  permissions?: MenuPermissions;
}

export interface Menu {
  route: string;
  name: string;
  type: 'link' | 'sub' | 'extLink' | 'extTabLink';
  icon: string;
  label?: MenuTag;
  badge?: MenuTag;
  children?: MenuChildrenItem[];
  permissions?: MenuPermissions;
}

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  private readonly menu$ = new BehaviorSubject<Menu[]>([]);

  /** Get all the menu data. */
  getAll() {
    return this.menu$.asObservable();
  }

  /** Observe the change of menu data. */
  change() {
    return this.menu$.pipe(share());
  }

  /** Initialize the menu data. */
  setMenu(menu: Menu[]): Observable<Menu[]> {
    this.menu$.next(menu);
    return this.menu$.asObservable();
  }

  /** Add one item to the menu data. */
  addMenuItem(item: Menu): void {
    const currentMenu = this.menu$.getValue();
    currentMenu.push(item);
    this.menu$.next(currentMenu);
  }

  /** Reset the menu data. */
  reset() {
    this.menu$.next([]);
  }

  /** Delete empty values and rebuild route. */
  buildRoute(routeArr: string[]) {
    let route = '';
    routeArr.forEach((item) => {
      if (item && item.trim()) {
        route += '/' + item.replace(/^\/+|\/+$/g, '');
      }
    });
    return route;
  }

  getLevel(routeArr: string[]) {
    let tmpArray: any[] = [];
    this.menu$.value.forEach((item) => {
      // Breadth-first traverse
      let unhandledLayer: Array<{
        item: any;
        parentNamePathList: string[];
        realRouteArr: string[];
      }> = [{ item, parentNamePathList: [], realRouteArr: [] }];
      while (unhandledLayer.length > 0) {
        let nextUnhandledLayer: Array<{
          item: any;
          parentNamePathList: string[];
          realRouteArr: string[];
        }> = [];
        for (const elem of unhandledLayer) {
          const eachItem = elem.item;
          const currentNamePathList = structuredClone(
            elem.parentNamePathList
          ).concat(eachItem.name);
          const currentRealRouteArr = structuredClone(
            elem.realRouteArr
          ).concat(eachItem.route);
          if(this.isRouteEqual(routeArr, currentRealRouteArr)) {
            tmpArray = currentNamePathList;
            break;
          }
          if(!this.isLeafitem(eachItem)) {
            const wrappedChildren = eachItem.children?.map((child : any) => ({
              item: child,
              parentNamePathList: currentNamePathList,
              realRouteArr: currentRealRouteArr,
            }));
            nextUnhandledLayer = nextUnhandledLayer.concat(wrappedChildren);
          }
        }
        unhandledLayer = nextUnhandledLayer;
      }
    });
    return tmpArray;
  }

  /** Check if the menu item is a leaf node. */
  private isLeafitem(item: MenuChildrenItem): boolean {
    const noRoute = !item.route || !item.route.trim();
    const noChildren = !item.children || item.children.length === 0;
    return noRoute && noChildren;
  }

  private isRouteEqual(routeArr: string[], realRouteArr: string[]) {
    const filteredRoute = routeArr.filter(r => r !== '');
    const filteredRealRoute = realRouteArr.filter(r => r !== '');
    return JSON.stringify(filteredRoute) === JSON.stringify(filteredRealRoute)
  }
}
