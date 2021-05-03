import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UploadXmiService } from '../services/upload-xmi.service';
import { DiagramService } from '../services/diagram-service.service';
import { DiagramRespone } from '../models/diagrams.response.model';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { ConfirmDiagramDeletionComponent } from '../confirm-diagram-deletion/confirm-diagram-deletion.component';
@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {
  ngOnInit(): void {

  }
  isLoading: boolean = false;
  diagramData: DiagramRespone[];
  constructor(private diagramService: DiagramService,
    public dialog: MatDialog) {
    this.isLoading = true;
    this.diagramService.getDiagrams().subscribe(result => {
      this.diagramData = result;
      this.isLoading = false;
      console.log(this.diagramData)
    })
  }
  deleteDiagram(diagramItem:DiagramRespone){
    const dialogRef = this.dialog.open(ConfirmDiagramDeletionComponent, {
      width: '250px',
      data: diagramItem
    });

    dialogRef.afterClosed().subscribe((result:DiagramRespone) => {
      if(result!=null){
        this.isLoading=true;
        this.diagramService.deleteDiagrams(result.id).subscribe(()=>{
          this.diagramService.getDiagrams().subscribe((resutlDiagrams=>{
            this.diagramData=resutlDiagrams;
            this.isLoading=false;
          }))
        })
      }
      
    });


  }


}
