import { Injectable } from '@angular/core';
import { MandelbrotWebApiService } from './mandelbrot-web-api.service';
import { CanvasSize } from '../model/canvas-size';
import { CanvasPosition } from '../model/canvas-position';
import { MandelbrotPosition } from '../model/mandelbrot-position';
import { MandelbrotSize } from '../model/mandelbrot-size';
import { MandelbrotResult } from '../model/mandelbrot-result';
import { MandelbrotParameter } from '../model/mandelbrot-parameter';
import { MandelbrotZoomParameter } from '../model/mandelbrot-zoom-parameter';
import { CanvasVector } from '../model/canvas-vector';
import { MandelbrotMoveParameter } from '../model/mandelbrot-move-parameter';
import { Semaphore } from './Semaphore';

@Injectable({
    providedIn: 'root',
})
export class MandelbrotService {
    private _semaphore: Semaphore = new Semaphore('semaphore');

    private _currentMandelbrotSize: MandelbrotSize;


    constructor(private mandebrotWebApi: MandelbrotWebApiService) {
        this._currentMandelbrotSize = new MandelbrotSize(
            new MandelbrotPosition(0.763, 0.0999),
            new MandelbrotPosition(0.768, 0.103)
        );
    }

    public async getInitialMandelbrotSet(imageData: ImageData, canvasSize: CanvasSize) : Promise<void> {
        const result
            = await this.mandebrotWebApi.getInitialMandelbrotSet(canvasSize.Width, canvasSize.Height, 255);
        if(!result.HasErrors)
            this.mapMandelbrotResult(result, imageData.data);

        this._currentMandelbrotSize = result.MandelbrotSize;
    }


    public async refreshedMandelbrotSet(
        imageData: ImageData,
        canvasSize: CanvasSize)
    : Promise<void> {
        const parameter = new MandelbrotParameter(canvasSize, this._currentMandelbrotSize, 255);
        const result = await this.mandebrotWebApi.getRefreshedMandelbrotSet(parameter);
        if(result.HasErrors)
            return;;

        this.mapMandelbrotResult(result, imageData.data);
        this._currentMandelbrotSize = result.MandelbrotSize;
    }

    public async zoomMandelbrotSet(
        imageData: ImageData,
        mousePosition: CanvasPosition,
        delta: number,
        canvasSize: CanvasSize)
    : Promise<void> {
        const zoomIn = delta > 0;

        const lock = await this._semaphore.acquire()

        const zoomParameter = new MandelbrotZoomParameter(mousePosition, zoomIn, canvasSize, this._currentMandelbrotSize, 255);
        const result = await this.mandebrotWebApi.zoomMandelbrotSet(zoomParameter);

        if(!result.HasErrors) {
            this.mapMandelbrotResult(result, imageData.data);
            this._currentMandelbrotSize = result.MandelbrotSize;
        }

        lock.release();
    }

    private count: number = 1;

    public async moveMandelbrotSetAsync(
        imageData: ImageData,
        startPosition: CanvasPosition,
        endPosition: CanvasPosition,
        canvasSize: CanvasSize)
    : Promise<void> {
        const vx = endPosition.X - startPosition.X;
        const vy = endPosition.Y - startPosition.Y;
        const mouseVector = new CanvasVector(vx, vy);

        const id = this.count++;

        const lock = await this._semaphore.acquire()

        const moveParameter = new MandelbrotMoveParameter(mouseVector, canvasSize, this._currentMandelbrotSize, 255);
        const result = await this.mandebrotWebApi.moveMandelbrotSetAsync(moveParameter);

        if(!result.HasErrors) {
            this.mapMandelbrotResult(result, imageData.data);
            this._currentMandelbrotSize = result.MandelbrotSize;
        }

        lock.release();
    }

    private mapMandelbrotResult(mandelbrotResult: MandelbrotResult, map: Uint8ClampedArray) {
        let index = 0;
        const codedData = mandelbrotResult.ImageData;
        for (let i = 0; i < codedData.length; i += 2) {
            const high = codedData[i];
            const low = codedData[i + 1];
            const color = this.color(this.convertToByte(high, low), 255);

            [map[index], map[index + 1], map[index + 2], map[index + 3]] = color;
            index += 4;
        }
    }

    private color(iteration: number, maxIteration: number): [number, number, number, number] {
        if (iteration >= maxIteration) {
                return [0, 0, 0, 255];
        }
        else
        {
            switch (iteration % 16 + 1)
            {
                case 1:  return [0, 0, 200, 255];
                case 2:  return [0, 0, 218, 255];
                case 3:  return [0, 0, 236, 255];
                case 4:  return [0, 0, 255, 255];

                case 5:  return [0, 255, 0, 255];
                case 6:  return [0, 236, 0, 255];
                case 7:  return [0, 218, 0, 255];
                case 8:  return [0, 200, 0, 255];

                case 9:  return [0, 0, 0, 255];
                case 10: return [218, 0, 0, 255];
                case 11: return [236, 0, 0, 255];
                case 12: return [255, 0, 0, 255];

                case 13: return [255, 255, 0, 255];
                case 14: return [236, 236, 0, 255];
                case 15: return [218, 218, 0, 255];
                case 16: return [200, 200, 0, 255];
                default: return [0, 0, 0, 255];
            }
        }
    }

    private convertToByte(hight: string, low: string): number {
        const value =
            (this.convertToNibble(hight) << 4) | this.convertToNibble(low);
        return value;
    }

    private convertToNibble(c: string): number {
        switch (c) {
            case '0':
                return 0;
            case '1':
                return 1;
            case '2':
                return 2;
            case '3':
                return 3;
            case '4':
                return 4;
            case '5':
                return 5;
            case '6':
                return 6;
            case '7':
                return 7;
            case '8':
                return 8;
            case '9':
                return 9;
            case 'A':
                return 10;
            case 'B':
                return 11;
            case 'C':
                return 12;
            case 'D':
                return 13;
            case 'E':
                return 14;
            case 'F':
                return 15;
            default:
                return 0;
        }
    }

    public MandelbrotPosition(
        canvasPosition: CanvasPosition,
        canvasSize: CanvasSize,
        mandelbrotSize: MandelbrotSize
    ): MandelbrotPosition {
        const mandelbrotMin = mandelbrotSize.Min;
        const mandelbrotMax = mandelbrotSize.Max;
        const mandelbrotX = mandelbrotMin.X + canvasPosition.X * (mandelbrotMax.X - mandelbrotMin.X) / (canvasSize.Width - 1);
        const mandelbrotY = mandelbrotMin.Y + canvasPosition.Y * (mandelbrotMax.Y - mandelbrotMin.Y) / (canvasSize.Height - 1);

        return new MandelbrotPosition(mandelbrotX, mandelbrotY);
    }
}
