import { InjectionToken } from "@angular/core";
import { IPostRepository } from "./repositories/post.repository";

export const PostRepository = new InjectionToken<IPostRepository>('PostRepository');
