import { TestBed } from '@angular/core/testing';

import { UploadXmiService } from './upload-xmi.service';

describe('UploadXmiService', () => {
  let service: UploadXmiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UploadXmiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
