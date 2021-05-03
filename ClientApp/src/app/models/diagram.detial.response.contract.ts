export interface DiagramDetailResponseContract {
        id: number;
        uploadName: string;
        authorEmail: string;
        diagramName: string;
        elements: string;
        createdDate: Date | string;
        updatedDate: Date | string;
    }