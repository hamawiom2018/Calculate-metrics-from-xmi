import { UMLAssociation, UMLAssociationObject, UMLAssociationPackage, UMLAttribute, UMLDependency, UMLDependencyObject, UMLDependencyPackage, UMLGeneralization, UMLObjectAttribute, UMLOperation, UMLPackageImport, UMLPackageMerge, UMLRelization, UMLUsage } from "./UMLElements";

export interface UMLElementRefined {
    Id: number;
    XmiId: string;
    Name: string;
    TreeName:string;
    Visibility: string;
    Type: number;
    sType: string;
    Childs: UMLElementRefined[];
    Parent: UMLElementRefined;
    IsElementDetail:boolean;
    IsOtherParty:boolean;
    IsHighlighted:boolean;
    OtherPartyRef:string;

}
export interface UMLPackageRefined extends UMLElementRefined {
    Associations: UMLAssociation[];
    Dependencies: UMLDependency[];
    Generalizations: UMLGeneralization[];
    Relizations: UMLRelization[];
    ImportedPackages: UMLPackageImport[];
    MergedPackage:UMLPackageMerge[];
    Usages:UMLUsage[];
}

export interface UMLClassRefined extends UMLElementRefined {
    Operations: UMLOperation[];
    Attributes: UMLAttribute[];
    Associations: UMLAssociation[];
    Dependencies: UMLDependency[];
    Generalizations: UMLGeneralization[];
    Relizations: UMLRelization[];
    Usages:UMLUsage[];
}

export interface UMLUseCaseRefined extends UMLElementRefined {
    Operations: UMLOperation[];
    Attributes: UMLAttribute[];
    Associations: UMLAssociation[];
    Dependencies: UMLDependency[];
    Generalizations: UMLGeneralization[];
    Relizations: UMLRelization[];
    Usages:UMLUsage[];
    StereoType:string;
}

export interface UMLObjectRefined extends UMLElementRefined {
    InstancedClassRef: string;
    InstancedClassName: string;
    Attributes: UMLObjectAttribute[];
    Operations: UMLOperation[];
    Associations: UMLAssociation[];
    Dependencies: UMLDependency[];
    Generalizations: UMLGeneralization[];
    Relizations: UMLRelization[];
}

export interface UMLLifeLineRefined extends UMLElementRefined {
    IncomingMessages: UMLMessageRefined[];
    OutgoingMessages: UMLMessageRefined[];
    ElementRef: string;
    ElmenetType: number;
    sElmenetType: string;
    ElementName: string;

}

export interface UMLMessageRefined extends UMLElementRefined {
    MessgaeKind:string;
    MessgaeSort:string;
    StereoType: string;
    ElementFromRef: string;
    ElementFromName: string;
    ElementFromType: number;
    sElementFromType: string;
    Direction:string;

    ElementToRef: string;
    ElementToName: string;
    ElementToType: number;
    sElementToType: string;
}