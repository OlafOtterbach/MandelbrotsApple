import { Injectable } from '@angular/core';
import { MandelbrotWebApiService } from './mandelbrot-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class MandelbrotService {

  constructor(private mandebrotWebApi: MandelbrotWebApiService) { }

  getGraphics(
    width: number,
    height: number,
    xMin: number,
    xMax: number,
    yMin: number,
    yMax: number,
    maxIterations: number) {
      this.mandebrotWebApi.getGraphics(width, height, xMin, xMax, yMin, yMax, maxIterations);
  }
}
