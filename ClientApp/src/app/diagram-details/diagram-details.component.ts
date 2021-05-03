import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { ActivatedRoute } from '@angular/router';
import { faChevronRight, faHome } from '@fortawesome/free-solid-svg-icons';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { convertUMLEmlementsRefined } from 'src/helpers/convert.uml.elements';
import { UMLType } from 'src/helpers/utils';
import { ElementMetricComponent } from '../element-metric/element-metric.component';
import { ElementMetric } from '../models/elelemt.metric';
import { UMLAssociation, UMLConnector, UMLDependency, UMLElement, UMLGeneralization, UMLInformationFlow, UMLMessage, UMLRelization, UMLTransition, UMLUsage } from '../models/elements/UMLElements';
import { UMLElementRefined, UMLLifeLineRefined, UMLMessageRefined } from '../models/elements/UMLElementsRefined';
import { EvaluateRequestContract } from '../models/evaluate.request.contract';
import { Metric } from '../models/metric.model';
import { DiagramService } from '../services/diagram-service.service';
import { MetricService } from '../services/metric.service';
import { MetricRequestContract } from '../models/metric.request.contract';

@Component({
  selector: 'app-diagram-details',
  templateUrl: './diagram-details.component.html',
  styleUrls: ['./diagram-details.component.css']
})
export class DiagramDetailsComponent implements OnInit {

  myControl = new FormControl();
  options: Metric[];
  filteredOptions: Observable<Metric[]>;

  
  isLoading: boolean = false;
  umlTypes = UMLType;
  private _transformer = (node: UMLElementRefined, level: number) => {
    return {
      expandable: !!node.Childs && node.Childs.length > 0,
      name: node.TreeName,
      level: level,
      selected: false,
      item: node

    };
  }
  selectedMetric:Metric;
  successUpload: boolean = false;
  responseData: any;
  selectedElement: UMLElementRefined;
  isLoadingEvaluate:boolean=false;
  evaluatedNumber:string='';
  isMetricEvaluated:boolean=false;
  treeControl = new FlatTreeControl<ExampleFlatNode>(
    node => node.level, node => node.expandable);
  treeFlattener = new MatTreeFlattener(
    this._transformer, node => node.level, node => node.expandable, node => node.Childs)

  resultElements: UMLElement[];
  currentParents: UMLElement[];
  currentElements: UMLElement[];
  currentElementsRefined: UMLElementRefined[];

  constructor(private route: ActivatedRoute,
    private diagramService: DiagramService,
    private metricService:MetricService,
    public dialog: MatDialog) { }
  faChevronRight = faChevronRight;
  faHome = faHome;
  filtered:string;
  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
  ngOnInit(): void {
    const routeParams = this.route.snapshot.paramMap;
    const diagramId = Number(routeParams.get('id'));
    this.isLoading = true;
    this.diagramService.getDiagramById(diagramId).subscribe(result => {
      console.log(result);
      this.currentElementsRefined = convertUMLEmlementsRefined(JSON.parse(result.elements), result.elements);
      this.dataSource.data = this.currentElementsRefined;
      this.selectedElement = null;
      this.responseData = result;
      const metricRequest:MetricRequestContract={DiagramId:this.responseData.id,TargetType:0}
      this.metricService.getMetric(metricRequest).subscribe(metricResult=>{
        this.options=metricResult;
        this.isLoading = false;
        this.filteredOptions = this.myControl.valueChanges.pipe(
          startWith(''),
          map(value => this._filter(value))
        );
        console.log(metricResult);
      })
      
    })

    


  }
  evaluate(){
    let evaluateRequestContract:EvaluateRequestContract={
      DiagramId:this.responseData.id,
      ElementRef: null,
      MetricCode:this.selectedMetric.metricCode
    }
    this.isLoadingEvaluate=true;
    this.metricService.evaluate(evaluateRequestContract).subscribe(result=>{
      this.isLoadingEvaluate=false;
      this.isMetricEvaluated=true;
      if(result!=-1){
        this.evaluatedNumber=result.toString();
      }else{
        this.evaluatedNumber="N/A"
      }
    })
  }
  getMetric(metric:Metric){
    this.selectedMetric=metric;
    this.isMetricEvaluated=false;
  }
  getOptionText(metric:Metric){
    return metric.metricCode;
  }
  private _filter(value: string): Metric[] {
    const filterValue = value;

    return this.options.filter(option => option.metricCode.toLowerCase().indexOf(filterValue) !== -1
    || option.metricDescription.toLowerCase().indexOf(filterValue) !== -1
    || option.metricName.toLowerCase().indexOf(filterValue) !== -1);
  }
  

  hasChild = (_: number, node: ExampleFlatNode) => node.expandable;

  selectTreeNode(item: any) {
    setTimeout(() => {
      this.dataSource._flattenedData.value.forEach(item2 => item2.selected = false);
      item.selected = true;
      this.selectedElement = item.item;
    }, 50);



  }
  calculateElementMetric(item:any){
    const elementMetric: ElementMetric={
      diagramId:this.responseData.id,
      element:item.item,
      resultElements:this.resultElements
    }
    const dialogRef = this.dialog.open(ElementMetricComponent, {
      width: '90%',
      data: elementMetric
    });
  }
  isElementDetail(item: ExampleFlatNode): boolean {
    return item.item.IsElementDetail == true;


  }
  unselected() {
    this.dataSource._flattenedData.value.forEach(item2 => {
      item2.selected = false;
      item2.item.IsHighlighted = false
    });
    this.selectedElement = null;
  }
  referenceOtherParty(item: any) {
    setTimeout(() => {
      let otherPartyRef: string = "";
      if (item.item.Type == UMLType.Transition || item.item.Type == UMLType.ControlFlow
        || item.item.Type == UMLType.ObjectFlow || item.item.Type == UMLType.InterruptingEdge) {
        let itemSelected = item.item as UMLTransition;

        if (itemSelected.SourceRef == itemSelected.Parent.XmiId) {
          otherPartyRef = itemSelected.TargetRef;
        } else {
          otherPartyRef = itemSelected.SourceRef;


        }
      } else if (item.item.Type == UMLType.Message) {
        let itemSelected = item.item as UMLMessageRefined;
        if (itemSelected.ElementFromRef == itemSelected.Parent.XmiId) {
          otherPartyRef = itemSelected.ElementToRef;
        } else {
          otherPartyRef = itemSelected.ElementFromRef;
        }
      } else if (item.item.Type == UMLType.LifeLine) {
        let itemSelected = item.item as UMLLifeLineRefined;
        otherPartyRef = itemSelected.ElementRef;

      } else if (item.item.Type == UMLType.Connector) {
        let itemSelected = item.item as UMLConnector;

        if (itemSelected.Parent.XmiId == itemSelected.Element1Ref) {
          otherPartyRef = itemSelected.Element2Ref;
        } else if (itemSelected.Parent.XmiId == itemSelected.Element2Ref) {
          otherPartyRef = itemSelected.Element1Ref;
        }
      } else if (item.item.Type == UMLType.InformationFlow) {
        let itemSelected = item.item as UMLInformationFlow;

        if (itemSelected.Parent.XmiId == itemSelected.SourceRef) {
          otherPartyRef = itemSelected.TargetName;
        } else if (itemSelected.Parent.XmiId == itemSelected.TargetRef) {
          otherPartyRef = itemSelected.SourceName;
        }
      } else if (item.item.Type == UMLType.Association) {
        let itemSelected = item.item as UMLAssociation;
        otherPartyRef=itemSelected.OtherPartyRef;
        
      }
      else if (item.item.Type == UMLType.Dependency) {
        let itemSelected = item.item as UMLDependency;

        if (itemSelected.ClientRef == itemSelected.Parent.XmiId) {
          otherPartyRef = itemSelected.SupplierRef;
        } else {
          otherPartyRef = itemSelected.ClientRef;
        }
      }else if (item.item.Type == UMLType.Generalization) {
        let itemSelected = item.item as UMLGeneralization;
        otherPartyRef=itemSelected.OtherPartyRef;
        
      }else if (item.item.Type == UMLType.Realization) {
        let itemSelected = item.item as UMLRelization;

        if (itemSelected.ClientRef == itemSelected.Parent.XmiId) {
          otherPartyRef = itemSelected.SupplierRef;
        } else {
          otherPartyRef = itemSelected.ClientRef;
        }
      }else if (item.item.Type == UMLType.Usage) {
        let itemSelected = item.item as UMLUsage;

        if (itemSelected.ClientRef == itemSelected.Parent.XmiId) {
          otherPartyRef = itemSelected.SupplierRef;
        } else {
          otherPartyRef = itemSelected.ClientRef;
        }
      }else if(item.item.OtherPartyRef!==""){
        otherPartyRef=item.item.OtherPartyRef;
      }
      if (otherPartyRef != "") {
        this.dataSource._flattenedData.value.forEach(item2 => item2.item.IsHighlighted = false);

        var referencedElement = this.dataSource._flattenedData.value.filter(item2 => item2.item.XmiId == otherPartyRef);
        if (referencedElement.length > 0) {
          let crrentElementExpanded = referencedElement[0];
          while (crrentElementExpanded.item.Parent != null) {
            let parentRef: string = crrentElementExpanded.item.Parent?.XmiId;
            if (parentRef != null) {
              var referencedParentElement = this.dataSource._flattenedData.value.filter(item2 => item2.item.XmiId == parentRef);
              this.treeControl.expand(referencedParentElement[0] as ExampleFlatNode);
              crrentElementExpanded = referencedParentElement[0];
            }

          }

          referencedElement[0].item.IsHighlighted = true;
          let el = document.getElementById(referencedElement[0].item.XmiId);
          el.scrollIntoView();
        }
      }


    }, 50);


  }

}
interface ExampleFlatNode {
  expandable: boolean;
  name: string;
  level: number;
  selected: boolean;
  item: UMLElementRefined;
  isElementDetail: boolean;
}