import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs/operators';
import { UploadXmiService } from '../services/upload-xmi.service';
import { UploadRequest } from '../models/upload.request.model';
import { ValidateExtension } from 'src/helpers/customvalidator.validator';
import { UploadResponse } from '../models/upload.response.model';
import { convertUMLEmlementsRefined } from 'src/helpers/convert.uml.elements';
import { UMLElement } from '../models/elements/UMLElements';
import { UniqueESSymbolType } from 'typescript';
import { UMLType } from 'src/helpers/utils';
import { faChevronRight, faHome } from '@fortawesome/free-solid-svg-icons'
import { from } from 'rxjs';
import { UMLElementRefined } from '../models/elements/UMLElementsRefined';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
@Component({
  selector: 'app-xmi-upload',
  templateUrl: './xmi-upload.component.html',
  styleUrls: ['./xmi-upload.component.css']
})
export class XmiUploadComponent implements OnInit {
  umlTypes = UMLType;
  private _transformer = (node: UMLElementRefined, level: number) => {
    return {
      expandable: !!node.Childs && node.Childs.length > 0,
      name:  node.TreeName,
      level: level,
      selected:false,
      item:node
      
    };
  }
  successUpload:boolean=false;
  responseData:any;
  selectedElement:UMLElementRefined;
  treeControl = new FlatTreeControl<ExampleFlatNode>(
    node => node.level, node => node.expandable);
    treeFlattener = new MatTreeFlattener(
      this._transformer, node => node.level, node => node.expandable, node => node.Childs)

  fileUpload: any;
  isLoading: boolean = false;
  validate: boolean = false;
  resultElements: UMLElement[];
  currentParents:UMLElement[];
  currentElements:UMLElement[];
  currentElementsRefined:UMLElementRefined[];
  
  uploadResponse: UploadResponse = { isSuccess: false, message: '', resultElements: '',
diagramName:'',name:'',xmlContent:''};
  xmiVersions: string[] = ["1.1", "2.1"]
  constructor(private uploadXmiService: UploadXmiService) { }
  myForm: FormGroup;
  get f() {
    return this.myForm.controls;
  }
  faChevronRight=faChevronRight;
  faHome=faHome;
  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  ngOnInit(): void {
    
    this.myForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(3)]),
      file: new FormControl('', [Validators.required, ValidateExtension]),
      fileSource: new FormControl('', [Validators.required])
    });
  }
  hasChild = (_: number, node: ExampleFlatNode) => node.expandable;

  navigateElement(element:UMLElement){
      let parent=element.Parent;
      this.currentParents=[];
      while(parent!=null){
        this.currentParents.push(parent);
        parent=parent.Parent;
      }
      this.currentParents.push(element);
      this.currentElements=element.Childs;
    
    
  }
  selectTreeNode(item:any){
    this.dataSource._flattenedData.value.forEach(item2=>item2.selected=false);
    item.selected=true;
    this.selectedElement=item.item;

    
  }
  isElementDetail(item:ExampleFlatNode):boolean{
    return item.item.IsElementDetail==true;

    
  }
  navigateToTop(){
    this.currentElements=this.resultElements;
    this.currentParents=[];
  }
  onFileChange(event) {

    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.myForm.patchValue({
        fileSource: file,
        file: file
      });
    }
  }

  uploadFile() {
    console.log(this.fileUpload);
  }
  _arrayBufferToBase64(buffer) {

    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
      binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
  }





  submit() {
    this.validate = true;
    this.resultElements=[];
    this.currentElements=[];
    this.currentParents=[];
    if (this.myForm.valid) {

      var reader = new FileReader();
      reader.onload = (ev: any) => {
        let base64 = this._arrayBufferToBase64(ev.target.result);


        const uploadItem: UploadRequest = {
          fileSource: base64,
          name: this.myForm.get('name').value
        }
        this.isLoading = true;
        this.uploadXmiService.addHero(uploadItem).subscribe(
          result => {
            this.isLoading = false;
            this.uploadResponse = result;
            console.log(result);
            if (result.resultElements != null) {
              //this.resultElements = convertUMLEmlements(JSON.parse(result.resultElements));
              //this.currentElements = convertUMLEmlements(JSON.parse(result.resultElements));
              this.currentElementsRefined = convertUMLEmlementsRefined(JSON.parse(result.resultElements),result.resultElements);
              this.dataSource.data=this.currentElementsRefined;
              this.selectedElement=null;
              this.responseData=result;
              console.log(this.currentElementsRefined);
              console.log(this.responseData);
              //console.log(test);
            }

          }, error => console.error(error));
      };
      reader.readAsArrayBuffer(this.myForm.get('fileSource').value);

    }

    /*
     this.http.post('http://localhost:8001/upload.php', formData)
       .subscribe(res => {
         console.log(res);
         alert('Uploaded Successfully.');
       })
       */
  }
  cancelUpload() {
    this.uploadResponse = { isSuccess: false, message: '', resultElements: '',diagramName:'',
  name:'',xmlContent:'' };
    this.resultElements = [];
  }
  saveData() {
    this.isLoading = true;
    const uploadItem: UploadRequest = {
      fileSource: this.uploadResponse.xmlContent,
      name: this.uploadResponse.name
    }
    this.uploadXmiService.addUpload(uploadItem).subscribe(result => {
      this.isLoading = false;
      this.successUpload=true;
      //this.uploadResponse = result;
      //console.log(result);
      

    }, error => console.error(error));
  }

}
interface ExampleFlatNode {
  expandable: boolean;
  name: string;
  level: number;
  selected:boolean;
  item:UMLElementRefined;
  isElementDetail:boolean;
}
