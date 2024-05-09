import { Observable } from 'rxjs';
import { Post } from '../models/post.model';


export interface IPostRepository {
    GetAllPostByTag(tags: string, sortBy?: string, direction?: string): Observable<Post[]>;
}
