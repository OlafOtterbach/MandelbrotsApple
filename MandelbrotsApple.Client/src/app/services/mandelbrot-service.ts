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

@Injectable({
    providedIn: 'root',
})
export class MandelbrotService {
    constructor(private mandebrotWebApi: MandelbrotWebApiService) {}

    public async getInitialMandelbrotSet(imageData: ImageData, canvasSize: CanvasSize) : Promise<MandelbrotSize> {
        const result
            = await this.mandebrotWebApi.getInitialMandelbrotSet(canvasSize.Width, canvasSize.Height, 255);
        if(!result.HasErrors)
            this.mapMandelbrotResult(result, imageData.data);

        return result.MandelbrotSize;
    }


    public async refreshedMandelbrotSet(
        imageData: ImageData,
        canvasSize: CanvasSize,
        mandelbrotSize: MandelbrotSize)
    : Promise<MandelbrotSize> {
        const parameter = new MandelbrotParameter(canvasSize, mandelbrotSize, 255);
        const result = await this.mandebrotWebApi.getRefreshedMandelbrotSet(parameter);
        if(result.HasErrors)
            return mandelbrotSize;

        this.mapMandelbrotResult(result, imageData.data);
        return result.MandelbrotSize;
    }

    public async zoomMandelbrotSet(
        imageData: ImageData,
        mousePosition: CanvasPosition,
        delta: number,
        canvasSize: CanvasSize,
        mandelbrotSize: MandelbrotSize)
    : Promise<MandelbrotSize> {
        const zoomIn = delta > 0;
        const zoomParameter = new MandelbrotZoomParameter(mousePosition, zoomIn, canvasSize, mandelbrotSize, 255);
        const result = await this.mandebrotWebApi.zoomMandelbrotSet(zoomParameter);
        if(result.HasErrors)
            return mandelbrotSize;

        this.mapMandelbrotResult(result, imageData.data);
        return result.MandelbrotSize;
    }

    public async moveMandelbrotSet(
        imageData: ImageData,
        startPosition: CanvasPosition,
        endPosition: CanvasPosition,
        canvasSize: CanvasSize,
        mandelbrotSize: MandelbrotSize)
    : Promise<MandelbrotSize> {
        const vx = endPosition.X - startPosition.X;
        const vy = endPosition.Y - startPosition.Y;
        const mouseVector = new CanvasVector(vx, vy);

        const zoomParameter = new MandelbrotMoveParameter(mouseVector, canvasSize, mandelbrotSize, 255);
        const result = await this.mandebrotWebApi.moveMandelbrotSet(zoomParameter);
        if(result.HasErrors)
            return mandelbrotSize;

        this.mapMandelbrotResult(result, imageData.data);
        return result.MandelbrotSize;
    }






    private mapMandelbrotResult(mandelbrotResult: MandelbrotResult, map: Uint8ClampedArray) {
        let index = 0;
        const codedData = mandelbrotResult.ImageData;
        for (let i = 0; i < codedData.length; i += 6) {
            const hightRed = codedData[i];
            const lowRed = codedData[i + 1];
            const red = this.convertToByte(hightRed, lowRed);
            const hightGreen = codedData[i + 2];
            const lowGreen = codedData[i + 3];
            const green = this.convertToByte(hightGreen, lowGreen);
            const hightBlue = codedData[i + 4];
            const lowBlue = codedData[i + 5];
            const blue = this.convertToByte(hightBlue, lowBlue);
            [map[index], map[index + 1], map[index + 2], map[index + 3]] = [
                red,
                green,
                blue,
                255,
            ];
            index += 4;
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

    public CanvasPosition(
        mandelbrotPosition: MandelbrotPosition,
        canvasSize: CanvasSize,
        mandelbrotSize: MandelbrotSize
    ): CanvasPosition {
        const mandelbrotMin = mandelbrotSize.Min;
        const mandelbrotMax = mandelbrotSize.Max;
        const canvasX = (mandelbrotPosition.X - mandelbrotMin.X) * (canvasSize.Width - 1) / (mandelbrotMax.X - mandelbrotMin.X);
        const canvasY = (mandelbrotPosition.Y - mandelbrotMin.Y) * (canvasSize.Height - 1) / (mandelbrotMax.Y - mandelbrotMin.Y);

        return new CanvasPosition(canvasX, canvasY);
    }
}
