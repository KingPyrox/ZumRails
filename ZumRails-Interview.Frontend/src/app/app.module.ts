import {  InjectionToken, NgModule } from '@angular/core';
import { BrowserModule, } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from "@angular/common/http";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PostTableComponent } from './post-table/post-table.component';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RestfulPostRepository } from './repositories/restful-post.repository';
import { PostRepository } from './injector.config';

@NgModule({
  declarations: [
        AppComponent,
        PostTableComponent
  ],
  imports: [
        BrowserModule,
      AppRoutingModule,
      BrowserAnimationsModule,
      HttpClientModule,
      MatTableModule,
      FormsModule,
      ReactiveFormsModule,
      MatSortModule,
      MatPaginatorModule,
      MatFormFieldModule,
      MatInputModule,
  ],
    providers: [
        { provide: PostRepository, useClass: RestfulPostRepository }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
