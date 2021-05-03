import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {  UploadRequest } from '../models/upload.request.model';
import { UploadResponse } from '../models/upload.response.model';

@Injectable({
  providedIn: 'root'
})
export class UploadXmiService {

  constructor(private http:HttpClient) { }
  addHero(uploadItem:UploadRequest): Observable<UploadResponse> {
    let header:HttpHeaders=new HttpHeaders();
    header.append('Content-Type','multipart/form-data');
    return this.http.post<UploadResponse>('/api/Upload/Post', uploadItem,{reportProgress:true,headers:header})
  }
  addUpload(uploadItem:UploadRequest): Observable<UploadResponse> {
    let header:HttpHeaders=new HttpHeaders();
    header.append('Content-Type','multipart/form-data');
    return this.http.post<UploadResponse>('/api/Upload/Save', uploadItem,{reportProgress:true,headers:header})
  }

  
  
  
}
