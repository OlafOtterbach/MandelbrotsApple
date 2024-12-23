import { MandelbrotParameter } from './../model/mandelbrot-parameter';
import { MandelbrotZoomParameter } from '../model/mandelbrot-zoom-parameter';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { MandelbrotResult } from '../model/mandelbrot-result';

@Injectable({
    providedIn: 'root',
})
export class MandelbrotWebApiService {
    constructor(private httpClient: HttpClient) {}

    public async getInitialMandelbrotSet(
        width: number,
        height: number,
        maxIterations: number)
    : Promise<MandelbrotResult> {
        const url = `http://localhost:5200/${width}/${height}/initialize`;
        const result = await lastValueFrom(this.httpClient.get<MandelbrotResult>(url));
        return result;
    }

    public async getRefreshedMandelbrotSet(
        parameter: MandelbrotParameter)
    : Promise<MandelbrotResult> {
        const url = `http://localhost:5200/refresh`;
        const result = await lastValueFrom(this.httpClient.post<MandelbrotResult>(url, parameter));
        return result;
    }

    public async zoomMandelbrotSet(
        zoomParameter: MandelbrotZoomParameter)
    : Promise<MandelbrotResult> {
        const url = `http://localhost:5200/zoom`;
        const result = await lastValueFrom(this.httpClient.post<MandelbrotResult>(url, zoomParameter));
        return result;
    }
}
