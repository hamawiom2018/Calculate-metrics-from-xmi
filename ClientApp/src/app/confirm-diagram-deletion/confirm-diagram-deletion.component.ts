import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DiagramRespone } from '../models/diagrams.response.model';

@Component({
  selector: 'app-confirm-diagram-deletion',
  templateUrl: './confirm-diagram-deletion.component.html',
  styleUrls: ['./confirm-diagram-deletion.component.css']
})
export class ConfirmDiagramDeletionComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ConfirmDiagramDeletionComponent>,
    @Inject(MAT_DIALOG_DATA) public diagram: DiagramRespone) { }

  ngOnInit(): void {
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

}
