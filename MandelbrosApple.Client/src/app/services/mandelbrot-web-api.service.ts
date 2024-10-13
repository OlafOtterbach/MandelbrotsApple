import { Injectable } from '@angular/core';
import { MandelbrotParameter } from './mandelbrot-parameter';


@Injectable({
  providedIn: 'root'
})
export class MandelbrotWebApiService {

constructor() { }

getGraphics(
  width: number,
  height: number,
  xMin: number,
  xMax: number,
  yMin: number,
  yMax: number,
  maxIterations: number) {
    const requestParam = new MandelbrotParameter(width, height, xMin, xMax, yMin, yMax, maxIterations);

  }
}
