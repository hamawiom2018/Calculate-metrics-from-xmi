import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DiagramDetailResponseContract } from '../models/diagram.detial.response.contract';
import { DeleteDiagramRequestContract, DiagramRespone } from '../models/diagrams.response.model';

@Injectable({
  providedIn: 'root'
})
export class DiagramService {

  constructor(private http: HttpClient) {

  }
  getDiagrams(): Observable<DiagramRespone[]> {
    let header: HttpHeaders = new HttpHeaders();
    return this.http.get<DiagramRespone[]>('/api/Diagram', { headers: header })
  }
  getDiagramById(id:number): Observable<DiagramDetailResponseContract> {
    let header: HttpHeaders = new HttpHeaders();
    return this.http.get<DiagramDetailResponseContract>('/api/Diagram/'+id, { headers: header })
  }
  deleteDiagrams(id: number): Observable<any> {
    let request: DeleteDiagramRequestContract = { Id: id };
    let header: HttpHeaders = new HttpHeaders();
    return this.http.delete<any>('/api/Diagram/'+id, { headers: header },)
  }
}
