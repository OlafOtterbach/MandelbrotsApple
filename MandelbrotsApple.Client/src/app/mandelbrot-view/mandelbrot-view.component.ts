import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { MandelbrotService } from '../services/mandelbrot-service';
import { MandelbrotSize } from '../model/mandelbrot-size';
import { CanvasPosition } from '../model/canvas-position';
import { CanvasSize } from '../model/canvas-size';
import { MandelbrotPosition } from '../model/mandelbrot-position';

@Component({
    selector: 'app-mandelbrot-view',
    standalone: true,
    templateUrl: './mandelbrot-view.component.html',
    styleUrls: ['./mandelbrot-view.component.css'],
})
export class MandelbrotViewComponent implements AfterViewInit {
    //@ViewChild('canvasId', { static: false }) canvasRef: ElementRef | undefined = undefined;
    @ViewChild('canvasId', { static: true }) canvasRef!: ElementRef;

    private canvas: HTMLCanvasElement | undefined;
    private context!: CanvasRenderingContext2D;

    private imageData!: ImageData;
    private canvasSize: CanvasSize = new CanvasSize(0, 0);
    private currentMandelbrotSize: MandelbrotSize;

    private mouseDown: boolean = false;
    private mouseMoved: boolean = false;
    private currentPosition: CanvasPosition = new CanvasPosition(-1, -1);

    constructor(private _mandelbrotService: MandelbrotService) {
        this.currentMandelbrotSize = new MandelbrotSize(
            new MandelbrotPosition(0.763, 0.0999),
            new MandelbrotPosition(0.768, 0.103)
        );
    }



    async ngAfterViewInit() {
        this.canvas = this.canvasRef.nativeElement as HTMLCanvasElement;
        if (this.canvas && this.canvas.getContext) {
            this.context = this.canvas.getContext('2d')!;

            this.updateCanvasData();
            this.currentMandelbrotSize = await this._mandelbrotService.getInitialMandelbrotSet(this.imageData, this.canvasSize);
            this.drawAsync();
        }
    }



    async onMouseDown(event: MouseEvent) {
        if (event.button === 0) {
            const position = this.canvasPosition(event.clientX, event.clientY);
            this.currentPosition = new CanvasPosition(position.X, position.Y);
            this.mouseDown = true;
            this.mouseMoved = false;
        }
    }

    onWheel(event: WheelEvent) {
        // const position = this.canvasPosition(event.clientX, event.clientY);
        // this.currentPosition = new CanvasPosition(position.X, position.Y);
        // this.zoomView(event.deltaY, this.currentPosition);

        // event.stopPropagation();
        // event.preventDefault();
    }

    // private zoomView(delta: number, mousePosition: CanvasPosition) {
    //     if (this.canvas === undefined) return;

    //     if (this.context === undefined) return;

    //     const canvasSize = this.getCanvasSize();

    //     const zoomFactor = delta < 0 ? 1.01 : 0.99;

    //     this.mandelbrotSize = this.zoomService(
    //         mousePosition,
    //         canvasSize,
    //         this.mandelbrotSize,
    //         zoomFactor
    //     );

    //     this.drawAsync();
    // }

    private async drawAsync() {
        this.context.putImageData(
            this.imageData,
            0,
            0,
            0,
            0,
            this.canvasSize.Width,
            this.canvasSize.Height
        );
    }

    public async onResize(_: Event) {
        this.updateCanvasData();
        this.currentMandelbrotSize = await this._mandelbrotService.getRefreshedMandelbrotSet(this.imageData, this.canvasSize, this.currentMandelbrotSize);
        this.drawAsync();
    }

    private canvasPosition(
        screenPositionX: number,
        screenpositionY: number
    ): CanvasPosition {
        if (this.canvas === undefined) return new CanvasPosition(-1, -1);

        const rect = this.canvas.getBoundingClientRect();
        const pos = new CanvasPosition(
            screenPositionX - rect.left,
            screenpositionY - rect.top
        );
        return pos;
    }

    private zoomService(
        mousePosition: CanvasPosition,
        canvasSize: CanvasSize,
        mandelbrotSize: MandelbrotSize,
        zoomFactor: number
    ): MandelbrotSize {
        const startMandelBrotPosition =
            this._mandelbrotService.MandelbrotPosition(
                mousePosition,
                canvasSize,
                mandelbrotSize
            );

        const xMin = mandelbrotSize.Min.X - startMandelBrotPosition.X;
        const yMin = mandelbrotSize.Min.Y - startMandelBrotPosition.Y;
        const xMax = mandelbrotSize.Max.X - startMandelBrotPosition.X;
        const yMax = mandelbrotSize.Max.Y - startMandelBrotPosition.Y;

        const newXMin = xMin * zoomFactor + startMandelBrotPosition.X;
        const newYMin = yMin * zoomFactor + startMandelBrotPosition.Y;
        const newXMax = xMax * zoomFactor + startMandelBrotPosition.X;
        const newYMax = yMax * zoomFactor + startMandelBrotPosition.Y;

        const zoomedMandelbrotSize = new MandelbrotSize(
            new MandelbrotPosition(newXMin, newYMin),
            new MandelbrotPosition(newXMax, newYMax)
        );
        return zoomedMandelbrotSize;
    }




    private updateCanvasData() {
        const newCanvasSize = this.getCanvasSize(this.context);
        if(newCanvasSize.Width != this.canvasSize.Width && newCanvasSize.Height != this.canvasSize.Height) {
            this.canvasSize = newCanvasSize;
            this.imageData = this.getImageData(this.canvasSize);
        }
    }

    private getCanvasSize(context: CanvasRenderingContext2D): CanvasSize {
        const width = context.canvas.width;
        const height = context.canvas.height;
        return new CanvasSize(width, height);
    }

    private getImageData(canvasSize: CanvasSize): ImageData {
        return this.context.getImageData(
            0,
            0,
            canvasSize.Width,
            canvasSize.Height
        );
    }
}
