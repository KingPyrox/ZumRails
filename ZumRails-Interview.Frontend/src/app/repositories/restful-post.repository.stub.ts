import { Observable, of } from "rxjs";
import { Post } from "../models/post.model";
import { IPostRepository } from "./post.repository";

export class RestulfulPostRepositoryStub implements IPostRepository {
    GetAllPostByTag(tags: string, sortBy?: string, direction?: string): Observable<Post[]> {
        return of([]);
    }
}
