import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Post } from "../models/post.model";
import { IPostRepository } from "./post.repository";


@Injectable()
export class RestfulPostRepository implements IPostRepository {

    private apiBaseUrl;
    constructor(private http: HttpClient) {
        this.apiBaseUrl = "https://localhost:7112/api/Posts"
    }

    GetAllPostByTag(tags: string, sortBy?: string, direction?: string): Observable<Post[]> {
        return this.http.get<Post[]>(`${this.apiBaseUrl}?tags=${tags}&sortBy=${sortBy}&direction=${direction}`);
    }


}
