import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { MandelbrotService } from '../services/mandelbrot-service';
import { ImagePosition } from '../model/image-position';
import { ImageSize } from '../model/image-size';
import { fromEvent, of } from 'rxjs';
import {
    throttleTime,
    debounceTime,
    filter,
    bufferTime,
    map,
    debounce,
    distinctUntilChanged,
    switchMap,
    scan,
    startWith,
    reduce,
    mergeMap,
} from 'rxjs/operators';

@Component({
    selector: 'app-mandelbrot-view',
    standalone: true,
    templateUrl: './mandelbrot-view.component.html',
    styleUrls: ['./mandelbrot-view.component.css'],
})
export class MandelbrotViewComponent implements AfterViewInit {
    @ViewChild('canvasId', { static: true }) canvasRef!: ElementRef;

    private canvas: HTMLCanvasElement | undefined;
    private context!: CanvasRenderingContext2D;

    private imageCanvas: HTMLCanvasElement | undefined;
    private imageContext: CanvasRenderingContext2D | null = null;
    private imageData!: ImageData;
    private imageSize: ImageSize = new ImageSize(1024, 1024); //new ImageSize(640, 480);
    private maxIterations: number = 1024;
    private currentPosition: ImagePosition = new ImagePosition(-1, -1);


    constructor(private _mandelbrotService: MandelbrotService) {}


    async ngAfterViewInit() {
        this.canvas = this.canvasRef.nativeElement as HTMLCanvasElement;
        if (this.canvas && this.canvas.getContext) {
            this.context = this.canvas.getContext('2d')!;

            this.imageCanvas = document.createElement('canvas');
            this.imageContext = this.imageCanvas.getContext('2d');
            this.imageCanvas.width = this.imageSize.Width;
            this.imageCanvas.height = this.imageSize.Height;
            this.imageData = this.getImageData(this.imageSize);

            await this._mandelbrotService.getInitialMandelbrotSet(
                this.imageSize,
                this.imageData,
                this.maxIterations
            );
            this.drawAsync();

            fromEvent<MouseEvent>(this.canvas, 'mousemove')
            .pipe(
              filter((event) => event.buttons === 1),
              throttleTime(100), // Nimmt alle Events innerhalb von 100ms den letzten
              switchMap(event => fromEvent<MouseEvent>(this.canvas!, 'mousemove').pipe(
                filter((event) => event.buttons === 1),
                debounceTime(300), // Wenn 300ms kein Event mehr kam, dann den letzten nehmen
                startWith(event) // Startet mit dem letzten Event von throttleTime
              ))
            )
            .subscribe((event) => this.onMouseMove(event));

            fromEvent<WheelEvent>(this.canvas, 'wheel')
            .pipe(
              bufferTime(200), // Sammle alle Events innerhalb von 100ms
              filter(events => events.length > 0), // Ignoriere leere Buffers
              map(events => events.reduce((acc, event) => {
                return {
                  x: event.clientX,
                  y: event.clientY,
                  delta: acc.delta + event.deltaY / 100
                };
              }, { x: 0, y: 0, delta: 0 })) // Addiere die Events auf
            )
            .subscribe((event) => this.onWheel(event));
        }
    }


    async onWheel(event: any) {
        const position = this.imagePosition(event.x, event.y);
        const delta = event.delta;

        await this._mandelbrotService.zoomMandelbrotSet(
            this.imageSize,
            this.imageData,
            this.maxIterations,
            position,
            delta
        );
        this.drawAsync();

        event.stopPropagation();
        event.preventDefault();
    }


    async onMouseDown(event: MouseEvent) {
        if (event.buttons !== 0) {
            const position = this.imagePosition(event.clientX, event.clientY);
            this.currentPosition = new ImagePosition(position.X, position.Y);
        }
    }


    async onMouseMove(event: MouseEvent) {
        const startPosition = this.currentPosition;
        const endPosition = this.imagePosition(event.clientX, event.clientY);
        this.currentPosition = endPosition;

        await this._mandelbrotService.moveMandelbrotSetAsync(
            this.imageSize,
            this.imageData,
            this.maxIterations,
            startPosition,
            endPosition
        );
        this.drawAsync();

        event.stopPropagation();
        event.preventDefault();
    }


    public async onResize(_: Event) {
        await this._mandelbrotService.refreshedMandelbrotSet(
            this.imageSize,
            this.imageData,
            this.maxIterations
        );
        this.drawAsync();
    }


    private async drawAsync() {
        if (this.imageContext != null)
            this.imageContext.putImageData(
                this.imageData,
                0,
                0,
                0,
                0,
                this.imageSize.Width,
                this.imageSize.Height
            );

        const canvasSize = this.getCanvasSize();
        if (this.imageCanvas)
            this.context.drawImage(
                this.imageCanvas,
                0,
                0,
                this.imageSize.Width,
                this.imageSize.Height,
                0,
                0,
                canvasSize.Width,
                canvasSize.Height
            );
    }


    private imagePosition(
        screenPositionX: number,
        screenpositionY: number
    ): ImagePosition {
        if (this.canvas === undefined) return new ImagePosition(-1, -1);

        const rect = this.canvas.getBoundingClientRect();
        const canvasPos = new ImagePosition(
            screenPositionX - rect.left,
            screenpositionY - rect.top
        );

        const canvasSize = this.getCanvasSize();
        const pos = new ImagePosition(
            canvasPos.X * (this.imageSize.Width / canvasSize.Width),
            canvasPos.Y * (this.imageSize.Height / canvasSize.Height)
        );

        return pos;
    }


    private getCanvasSize(): ImageSize {
        const width = this.context.canvas.width;
        const height = this.context.canvas.height;
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
