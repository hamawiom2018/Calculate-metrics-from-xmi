export interface Metric {
    id: number;
    metricCode: string;
    metricName: string;
    metricDescription: string;
    targetType: number;
    targetTypeName: string;
    createdDate: Date | string;
    updatedDate: Date | string;
}