import { UMLElementRefined } from "./UMLElementsRefined";

export interface UMLElement {
    Id: number;
    XmiId: string;
    Name: string;
    Visibility: string;
    Type: number;
    Childs: UMLElement[];
    Parent: UMLElement;
}

export interface UMLAttribute extends UMLElement {
    IsClassDataType: boolean;
    DataTypeName: string;
    IsArray: boolean;
    Multiplicity: string;
}

export interface UMLAssociation extends UMLElementRefined {
    Element1Ref: string;
    Element1Name: string;
    Element1Aggregation: string;
    Element1Multiplicity: string;
    Element1Type: number;
    sElement1Type: string;

    Element2Ref: string;
    Element2Name: string;
    Element2Aggregation: string;
    Element2Multiplicity: string;
    Element2Type: number;
    sElement2Type: string;
    StereoType: string;
}
export interface UMLOperationParameter extends UMLElement {
    IsClassDataType: boolean;
    DataTypeName: string;
    IsArray: boolean;
    Kind: string;
}
export interface UMLOperation extends UMLElement {
    IsClassDataType: boolean;
    DataTypeName: string;
    IsArray: boolean;
    Parameters: UMLOperationParameter[];
}
export interface UMLClass extends UMLElement {
    Operations: UMLOperation[];
    Attributes: UMLAttribute[];
}
export interface UMLDependency extends UMLElementRefined {
    ClientRef: string;
    ClientName: string;
    ClientType: number;
    sClientType: string;
    SupplierRef: string;
    SupplierName: string;
    SupplierType: number;
    sSupplierType: string;
    StereoType: string;
}
export interface UMLGeneralization extends UMLElementRefined {
    ParentRef: string;
    ParentName: string;
    ParentType: number;
    sParentType: string;
    ChildRef: string;
    ChildName: string;
    ChildType: number;
    sChildType: string;
}
export interface UMLRelization extends UMLElementRefined {
    ClientRef: string;
    ClientName: string;
    ClientType: number;
    sClientType: string;
    SupplierRef: string;
    SupplierName: string;
    SupplierType: number;
    sSupplierType: string;
    StereoType: string;
}

export interface UMLAssociationObject extends UMLElement {
    Object1Ref: string;
    Object1Name: string;
    Object1Aggregation: string;
    Object1Multiplicity: string;
    Object2Ref: string;
    Object2Name: string;
    Object2Aggregation: string;
    Object2Multiplicity: string;
    StereoType: string;
}

export interface UMLObjectAttribute extends UMLElement {
    isBodyValue: boolean;
    Value: string;
}

export interface UMLObject extends UMLElement {
    InstancedClassRef: string;
    InstancedClassName: string;
    Attributes: UMLObjectAttribute[];
}
export interface UMLDependencyPackage extends UMLElement {
    ClientPackageRef: string;
    ClientPackageName: string;
    SupplierPackageRef: string;
    SupplierPackageName: string;
    StereoType: string;
}
export interface UMLAssociationPackage extends UMLElement {
    Package1Ref: string;
    Package1Name: string;
    Package1Aggregation: string;
    Package1Multiplicity: string;
    Package2Ref: string;
    Package2Name: string;
    Package2Aggregation: string;
    Package2Multiplicity: string;
    StereoType: string;
}
export interface UMLDependencyObject extends UMLElement {
    ClientObjectRef: string;
    ClientObjectName: string;
    SupplierObjectRef: string;
    SupplierObjectName: string;
    StereoType: string;
}
export interface UMLPackageImport extends UMLElementRefined {
    ImportedPackageRef: string;
    ImportedPackageName: string;
}

export interface UMLPackageMerge extends UMLElementRefined {
    MergedPackageRef: string;
    MergedPackageName: string;
}
export interface UMLUsage extends UMLElementRefined {
    ClientRef: string;
    ClientName: string;
    ClientType: number;
    sClientType: string;
    SupplierRef: string;
    SupplierName: string;
    SupplierType: number;
    sSupplierType: string;
    StereoType: string;
}

export interface UMLLifeLine extends UMLElement {
    IncomingMessages: UMLMessage[];
    OutgoingMessages: UMLMessage[];
    ElementRef: string;
    ElmenetType: number;
    sElmenetType: string;
    ElementName: string;

}

export interface UMLMessage extends UMLElementRefined {
    MessgaeKind: string;
    MessgaeSort: string;
    StereoType: string;
    ElementFromRef: string;
    ElementFromName: string;
    ElementFromType: number;
    sElementFromType: string;

    ElementToRef: string;
    ElementToName: string;
    ElementToType: number;
    sElementToType: string;
}

export interface UMLCombinedFragment extends UMLElementRefined {
    InteractionOperator: string;
}

export interface UMLInteractionOperand extends UMLElementRefined {
    Expression: string;

}

export interface UMLState extends UMLElementRefined {
    StateType: number;
    Kind: string;
    Entry: string;
    Do: string;
    Exit: string;
}

export interface UMLTransition extends UMLElementRefined {
    SourceRef: string;
    SourceName: string;
    SourceType: number;
    sSourceType: string;


    TargetRef: string;
    TargetName: string;
    TargetType: number;
    sTargetType: string;
    Trigger:string;
    Guard:string;
}

export interface UMLPort extends UMLElementRefined {
        Aggregation: string;
    }
    export interface UMLProperty extends UMLElementRefined {
        PropertyRef: string;
        PropertyName: string;
        PropertyType: number;
        sPropertyType: string;
    }
    export interface UMLProvided extends UMLElementRefined {
        ProvidedRef: string;
        ProvidedName: string;
        ProvidedType: number;
        sProvidedType: string;
    }
    export interface UMLRequired extends UMLElementRefined {
        RequiredRef: string;
        RequiredName: string;
        RequiredType: number;
        sRequiredType: string;
    }
    export interface UMLInformationFlow extends UMLElementRefined {
        SourceRef: string;
        SourceName: string;
        SourceType: number;
        sSourceType: string;


        TargetRef: string;
        TargetName: string;
        TargetType: number;
        sTargetType: string;

    }

    export interface UMLConnector extends UMLElementRefined {
        Element1Ref: string;
        Element1Name: string;
        Element1Type: number;
        sElement1Type: string;
        Element2Ref: string;
        Element2Name: string;
        Element2Type: number;
        sElement2Type: string;
        Kind: string;
    }