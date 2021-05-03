import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ElementMetric } from '../models/elelemt.metric';
import { EvaluateRequestContract } from '../models/evaluate.request.contract';
import { Metric } from '../models/metric.model';
import { MetricRequestContract } from '../models/metric.request.contract';
import { MetricService } from '../services/metric.service';

@Component({
  selector: 'app-element-metric',
  templateUrl: './element-metric.component.html',
  styleUrls: ['./element-metric.component.css']
})
export class ElementMetricComponent implements OnInit {

  myControl = new FormControl();
  options: Metric[];
  filteredOptions: Observable<Metric[]>;
  isLoading: boolean = false;
  selectedMetric: Metric;
  isMetricEvaluated = false;
  isLoadingEvaluate: boolean = false;
  evaluatedNumber: string = '';
  constructor(public dialogRef: MatDialogRef<ElementMetricComponent>,
    @Inject(MAT_DIALOG_DATA) public elementRef: ElementMetric,
    private metricService: MetricService
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    const metricRequest: MetricRequestContract = {
      DiagramId: this.elementRef.diagramId,
      TargetType: this.elementRef.element.Type
    };
    this.metricService.getMetric(metricRequest).subscribe(result => {
      this.options = result;
      this.filteredOptions = this.myControl.valueChanges.pipe(
        startWith(''),
        map(value => this._filter(value))
      );
      this.isLoading = false;

    })
  }
  private _filter(value: string): Metric[] {
    const filterValue = value;

    return this.options.filter(option => option.metricCode.toLowerCase().indexOf(filterValue) !== -1
      || option.metricDescription.toLowerCase().indexOf(filterValue) !== -1
      || option.metricName.toLowerCase().indexOf(filterValue) !== -1);
  }
  getMetric(metric: Metric) {
    this.selectedMetric = metric;
    this.isMetricEvaluated = false;
  }
  getOptionText(metric: Metric) {
    return metric.metricCode;
  }
  evaluate() {
    let evaluateRequestContract: EvaluateRequestContract = {
      DiagramId: this.elementRef.diagramId,
      ElementRef: this.elementRef.element.XmiId,
      MetricCode: this.selectedMetric.metricCode
    }
    this.isLoadingEvaluate = true;
    this.metricService.evaluate(evaluateRequestContract).subscribe(result => {
      this.isLoadingEvaluate = false;
      this.isMetricEvaluated = true;
      if (result != -1) {
        this.evaluatedNumber = result.toString();
      } else {
        this.evaluatedNumber = "N/A"
      }
    })
  }

}
