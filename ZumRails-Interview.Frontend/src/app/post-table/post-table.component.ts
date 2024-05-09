import { Component, Inject, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { Post } from '../models/post.model';
import { catchError, map, merge, startWith, switchMap, of as observableOf } from 'rxjs';
import { IPostRepository } from '../repositories/post.repository';
import { PostRepository } from '../injector.config';

@Component({
    selector: 'app-post-table',
    templateUrl: './post-table.component.html',
    styleUrl: './post-table.component.scss'
})
export class PostTableComponent {
    tags: string;
    @ViewChild(MatSort) sort: MatSort;

    displayedColumns: string[] = ['id', 'author', 'authorId', 'likes', 'popularity', 'reads', 'tags'];
    postDatabase: IPostRepository | null;
    data: Post[] = [];

    constructor(@Inject(PostRepository) private postRepo: IPostRepository) {
        this.postDatabase = this.postRepo;
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.tags = filterValue.trim().toLowerCase();

        if (this.tags.length > 3)
            this.loadData();
    }

    loadData() {
        if (this.tags == null)
            return;
        merge(this.sort.sortChange)
            .pipe(
                startWith({}),
                switchMap(() => {
                    return this.postDatabase!.GetAllPostByTag(
                        this.tags,
                        this.sort.active,
                        this.sort.direction,
                    ).pipe(catchError(((error) => {
                        console.error('Error fetching data:', error);
                        return observableOf([]);
                    })));
                }),
                map(data => {
                    if (data === null) {
                        return [];
                    }
                    return data;
                }),
            )
            .subscribe(data => {
                this.data = data;
            });
    }
}
