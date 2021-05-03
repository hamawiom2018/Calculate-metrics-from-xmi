import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EvaluateRequestContract } from '../models/evaluate.request.contract';
import { Metric } from '../models/metric.model';
import { MetricRequestContract } from '../models/metric.request.contract';

@Injectable({
  providedIn: 'root'
})
export class MetricService {

  constructor(private http: HttpClient) { }
  getMetric(model:MetricRequestContract): Observable<Metric[]> {
    let header: HttpHeaders = new HttpHeaders();
    return this.http.post<Metric[]>('/api/Metric/GetMetric',model, { headers: header })
  }
  evaluate(requestContract:EvaluateRequestContract): Observable<number> {
    let header: HttpHeaders = new HttpHeaders();
    return this.http.post<number>('/api/Metric/Evaluate',requestContract, { headers: header })
  }
}
