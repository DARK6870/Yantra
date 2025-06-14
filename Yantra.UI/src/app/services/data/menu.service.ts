import { inject, Injectable } from '@angular/core';
import { GraphqlHelperService } from '../graphql/graphql-helper.service';
import {Observable} from 'rxjs';
import {MenuItem} from '../../models/MenuItem';
import {GET_MENU_ITEMS} from '../../graphql.operations';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private readonly _graphqlHelper = inject(GraphqlHelperService);
  constructor() {
  }

  getMenuItems(): Observable<MenuItem> {
    return this._graphqlHelper.sendQuery<MenuItem>(GET_MENU_ITEMS);
  }
}
