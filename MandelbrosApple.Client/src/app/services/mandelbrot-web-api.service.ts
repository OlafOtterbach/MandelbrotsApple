import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MandelbrotParameter } from './mandelbrot-parameter';
import { lastValueFrom } from 'rxjs';
import { MandelbrotResult } from './mandelbrot-result';


@Injectable({
  providedIn: 'root'
})
export class MandelbrotWebApiService {

constructor(private httpClient: HttpClient) { }

public async getGraphics(
  width: number,
  height: number,
  xMin: number,
  xMax: number,
  yMin: number,
  yMax: number,
  maxIterations: number) : Promise<MandelbrotResult> {
    const requestParam = new MandelbrotParameter(width, height, xMin, xMax, yMin, yMax, maxIterations);

    const url = "http://localhost:5200/mandelbrot";
    const result = await lastValueFrom(this.httpClient.post<MandelbrotResult>(url, requestParam));
    return result;
  }
}
