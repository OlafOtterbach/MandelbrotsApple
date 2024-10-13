/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MandelbrotViewComponent } from './mandelbrot-view.component';

describe('MandelbrotViewComponent', () => {
  let component: MandelbrotViewComponent;
  let fixture: ComponentFixture<MandelbrotViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MandelbrotViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MandelbrotViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
