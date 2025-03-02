import { Injectable } from '@angular/core';
import { MandelbrotWebApiService } from './mandelbrot-web-api.service';
import { ImageSize } from '../model/image-size';
import { ImagePosition } from '../model/image-position';
import { MandelbrotPosition } from '../model/mandelbrot-position';
import { MandelbrotSize } from '../model/mandelbrot-size';
import { MandelbrotResult } from '../model/mandelbrot-result';
import { MandelbrotParameter } from '../model/mandelbrot-parameter';
import { MandelbrotZoomParameter } from '../model/mandelbrot-zoom-parameter';
import { ImageVector } from '../model/image-vector';
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


    public async getInitialMandelbrotSet(imageSize: ImageSize ,imageData: ImageData, maxIterations: number) : Promise<void> {
        const result = await this.mandebrotWebApi.getInitialMandelbrotSet(imageSize.Width, imageSize.Height, maxIterations);
        if(!result.HasErrors)
            this.mapMandelbrotResult(result, imageData.data);

        this._currentMandelbrotSize = result.MandelbrotSize;
    }


    public async refreshedMandelbrotSet(imageSize: ImageSize ,imageData: ImageData, maxIterations: number) : Promise<void> {
        const parameter = new MandelbrotParameter(imageSize, this._currentMandelbrotSize, maxIterations);
        const result = await this.mandebrotWebApi.getRefreshedMandelbrotSet(parameter);
        if(result.HasErrors)
            return;;

        this.mapMandelbrotResult(result, imageData.data);
        this._currentMandelbrotSize = result.MandelbrotSize;
    }


    public async zoomMandelbrotSet(
        imageSize: ImageSize,
        imageData: ImageData,
        maxIterations: number,
        mousePosition: ImagePosition,
        delta: number)
    : Promise<void> {
        const zoomIn = delta > 0;
        const zoomCount = Math.abs(delta);

        const lock = await this._semaphore.acquire()

        const zoomParameter = new MandelbrotZoomParameter(mousePosition, zoomIn, zoomCount, imageSize, this._currentMandelbrotSize, maxIterations);
        const result = await this.mandebrotWebApi.zoomMandelbrotSet(zoomParameter);

        if(!result.HasErrors) {
            this.mapMandelbrotResult(result, imageData.data);
            this._currentMandelbrotSize = result.MandelbrotSize;
        }

        lock.release();
    }


    public async moveMandelbrotSetAsync(
        imageSize: ImageSize,
        imageData: ImageData,
        maxIterations: number,
        startPosition: ImagePosition,
        endPosition: ImagePosition)
    : Promise<void> {
        const vx = endPosition.X - startPosition.X;
        const vy = endPosition.Y - startPosition.Y;
        const mouseVector = new ImageVector(vx, vy);

        const lock = await this._semaphore.acquire()

        const moveParameter = new MandelbrotMoveParameter(mouseVector, imageSize, this._currentMandelbrotSize, maxIterations);
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
        for (let i = 0; i < codedData.length; i += mandelbrotResult.BytesPerPixel * 2) {
            let num = 0;

            for(let j = 0; j < mandelbrotResult.BytesPerPixel * 2; j += 2) {
                const high = codedData[i + j];
                const low = codedData[i + j + 1];
                const byte = this.convertToByte(high, low);
                num = num << 8 | byte;
            }

            const color = this.color(num, mandelbrotResult.MaxIterations);
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

                case 9:  return [200, 0, 0, 255];
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
        imagePosition: ImagePosition,
        canvasSize: ImageSize,
        mandelbrotSize: MandelbrotSize
    ): MandelbrotPosition {
        const mandelbrotMin = mandelbrotSize.Min;
        const mandelbrotMax = mandelbrotSize.Max;
        const mandelbrotX = mandelbrotMin.X + imagePosition.X * (mandelbrotMax.X - mandelbrotMin.X) / (canvasSize.Width - 1);
        const mandelbrotY = mandelbrotMin.Y + imagePosition.Y * (mandelbrotMax.Y - mandelbrotMin.Y) / (canvasSize.Height - 1);

        return new MandelbrotPosition(mandelbrotX, mandelbrotY);
    }
}
