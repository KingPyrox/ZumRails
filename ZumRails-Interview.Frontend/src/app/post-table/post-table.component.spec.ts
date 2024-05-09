import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostTableComponent } from './post-table.component';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RestulfulPostRepositoryStub } from '../repositories/restful-post.repository.stub';
import { InjectionToken, NO_ERRORS_SCHEMA } from '@angular/core';
import { IPostRepository } from '../repositories/post.repository';
import { HttpClientModule } from '@angular/common/http';

export const PostRepository = new InjectionToken<IPostRepository>('PostRepository');

describe('PostTableComponent', () => {
  let component: PostTableComponent;
    let fixture: ComponentFixture<PostTableComponent>;
    let repo = new RestulfulPostRepositoryStub();
    
    beforeEach(async () => {
        spyOn(repo, 'GetAllPostByTag').and.callThrough();
    await TestBed.configureTestingModule({
        declarations: [PostTableComponent],
        imports: [
            HttpClientModule,
            MatTableModule,
            MatSortModule,
            MatFormFieldModule,
            MatInputModule
        ],
        providers: [
            { provide: PostRepository, useValue: repo }
        ],
        schemas: [NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  });

    it('should create', () => {
        fixture = TestBed.createComponent(PostTableComponent);
        component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });

    function querySelector(querySelector: string) {
        return fixture.debugElement.nativeElement.querySelector(querySelector)
    }
});


