import { UMLElement } from "./elements/UMLElements";
import { UMLElementRefined } from "./elements/UMLElementsRefined";

export interface ElementMetric{
    element:UMLElementRefined;
    resultElements:UMLElement[];
    diagramId:number;
}