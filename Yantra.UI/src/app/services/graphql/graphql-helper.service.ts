import { Injectable } from '@angular/core';
import { Apollo, QueryRef } from 'apollo-angular';
import { DocumentNode } from 'graphql';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GraphqlHelperService {

  constructor(private apollo: Apollo) { }

  sendQuery<T>(query: DocumentNode, variables?: { [key: string]: any }): Observable<T> {
    const queryRef: QueryRef<T, any> = this.apollo.watchQuery<T>({
      query,
      variables
    });

    return queryRef.valueChanges.pipe(
      map(result => {
        if (result.errors) {
          throw new Error('GraphQL Error: ' + JSON.stringify(result.errors));
        }
        return result.data;
      }),
      catchError(err => {
        console.error('GraphQL Query Error:', err);
        throw err; // you can customize or rethrow
      })
    );
  }
}
