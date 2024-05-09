import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { RestulfulPostRepositoryStub } from './repositories/restful-post.repository.stub';
import { PostTableComponent } from './post-table/post-table.component';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { InjectionToken, NO_ERRORS_SCHEMA } from '@angular/core';
import { IPostRepository } from './repositories/post.repository';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

export const PostRepository = new InjectionToken<IPostRepository>('PostRepository');

describe('AppComponent', () => {
    let fixture: ComponentFixture<AppComponent>;
    let component: AppComponent;
    let repo = new RestulfulPostRepositoryStub();

    beforeEach(async () => {
        spyOn(repo, 'GetAllPostByTag').and.callThrough();
      await TestBed.configureTestingModule({
          declarations: [
              AppComponent,
              PostTableComponent
          ],
            imports: [
                BrowserModule,
                BrowserAnimationsModule,
                AppRoutingModule,
                BrowserAnimationsModule,
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
      }).compileComponents();

        fixture = TestBed.createComponent(AppComponent);
      component = fixture.componentInstance;
  });

  it('should create the app', () => {
      expect(component).toBeTruthy();
  });

  it(`should have as title 'ZumRails-Interview.Frontend'`, () => {
      expect(component.title).toEqual('ZumRails-Interview.Frontend');
  });

  it('should render title', () => {
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hello, ZumRails-Interview.Frontend');
  });

  xit('should render table component', () => {
    fixture.detectChanges();
      const compiled = fixture.nativeElement as HTMLElement;
      expect(compiled.querySelector('app-post-table')?.textContent).toBeTruthy();
  });
});
