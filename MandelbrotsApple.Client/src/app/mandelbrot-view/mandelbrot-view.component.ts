import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { MandelbrotService } from '../services/mandelbrot-service';
import { MandelbrotSize } from '../model/mandelbrot-size';
import { ImagePosition } from '../model/image-position';
import { ImageSize } from '../model/image-size';
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
    private imageSize: ImageSize = new ImageSize(0, 0);

    private mouseDown: boolean = false;
    private mouseMoved: boolean = false;
    private currentPosition: ImagePosition = new ImagePosition(-1, -1);

    constructor(private _mandelbrotService: MandelbrotService) {
    }



    async ngAfterViewInit() {
        this.canvas = this.canvasRef.nativeElement as HTMLCanvasElement;
        if (this.canvas && this.canvas.getContext) {
            this.context = this.canvas.getContext('2d')!;

            this.updateCanvasData();
            await this._mandelbrotService.getInitialMandelbrotSet(this.imageData, this.imageSize);
            this.drawAsync();
        }
    }



    async onMouseDown(event: MouseEvent) {
        if (event.buttons !== 0) {
            const position = this.imagePosition(event.clientX, event.clientY);
            this.currentPosition = new ImagePosition(position.X, position.Y);
            //console.log("Pos("+this.currentPosition.X+","+this.currentPosition.Y+")");
            this.mouseDown = true;
            this.mouseMoved = false;
        }
    }

    async onMouseUp() {
        this.mouseDown = false;
        this.mouseMoved = false;
    }

    async onMouseMove(event: MouseEvent) {
        if (this.mouseDown == true) {
            this.mouseMoved = true;

            this.updateCanvasData();
            const startPosition = this.currentPosition;
            const endPosition = this.imagePosition(event.clientX, event.clientY);
            this.currentPosition = endPosition;

            //console.log("Pos("+endPosition.X+","+endPosition.Y+")");

            await this._mandelbrotService.moveMandelbrotSetAsync(this.imageData, startPosition, endPosition, this.imageSize);
            this.drawAsync();

            event.stopPropagation();
            event.preventDefault();
        }
    }

    async onWheel(event: WheelEvent) {
        const position = this.imagePosition(event.clientX, event.clientY);
        const delta = event.deltaY;

        this.updateCanvasData();
        await this._mandelbrotService.zoomMandelbrotSet(this.imageData, position, delta, this.imageSize);
        this.drawAsync();

        event.stopPropagation();
        event.preventDefault();
    }

    private async drawAsync() {
        this.context.putImageData(
            this.imageData,
            0,
            0,
            0,
            0,
            this.imageSize.Width,
            this.imageSize.Height
        );
    }

    public async onResize(_: Event) {
        this.updateCanvasData();
        await this._mandelbrotService.refreshedMandelbrotSet(this.imageData, this.imageSize);
        this.drawAsync();
    }





    private imagePosition(
        screenPositionX: number,
        screenpositionY: number
    ): ImagePosition {
        if (this.canvas === undefined) return new ImagePosition(-1, -1);

        const rect = this.canvas.getBoundingClientRect();
        const pos = new ImagePosition(
            screenPositionX - rect.left,
            screenpositionY - rect.top
        );
        return pos;
    }

    private updateCanvasData() {
        const newCanvasSize = this.getCanvasSize(this.context);
        if(newCanvasSize.Width != this.imageSize.Width && newCanvasSize.Height != this.imageSize.Height) {
            this.imageSize = newCanvasSize;
            this.imageData = this.getImageData(this.imageSize);
        }
    }

    private getCanvasSize(context: CanvasRenderingContext2D): ImageSize {
        const width = context.canvas.width;
        const height = context.canvas.height;
        return new ImageSize(width, height);
    }

    private getImageData(imageSize: ImageSize): ImageData {
        return this.context.getImageData(
            0,
            0,
            imageSize.Width,
            imageSize.Height
        );
    }
}
