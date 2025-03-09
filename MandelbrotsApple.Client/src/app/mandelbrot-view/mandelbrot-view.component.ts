import {
    AfterViewInit,
    Component,
    ElementRef,
    HostListener,
    ViewChild,
} from '@angular/core';
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
    sampleTime,
} from 'rxjs/operators';

@Component({
    selector: 'app-mandelbrot-view',
    standalone: true,
    templateUrl: './mandelbrot-view.component.html',
    styleUrls: ['./mandelbrot-view.component.scss'],
})
export class MandelbrotViewComponent implements AfterViewInit {
    @ViewChild('canvasId', { static: true }) canvasRef!: ElementRef;

    private canvas: HTMLCanvasElement | undefined;
    private context!: CanvasRenderingContext2D;

    private imageCanvas: HTMLCanvasElement | undefined;
    private imageContext: CanvasRenderingContext2D | null = null;
    private imageData!: ImageData;
    private imageBase = 1024;
    private imageSize: ImageSize = new ImageSize(this.imageBase, this.imageBase);
    private maxIterations: number = 255;
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
            this.onResizeSlow(new Event('resize'));

            fromEvent<MouseEvent>(this.canvas, 'mousemove')
                .pipe(
                    filter((event) => event.buttons === 1),
                    sampleTime(200), // Nimmt von allen Events innerhalb von 100ms den Letzten
                )
                .subscribe((event) => this.onMouseMove(event));


            fromEvent<WheelEvent>(this.canvas, 'wheel')
                .pipe(
                    bufferTime(200), // Sammle alle Events innerhalb von 100ms
                    filter((events) => events.length > 0), // Ignoriere leere Buffers
                    map((events) =>
                        events.reduce(
                            (acc, event) => {
                                return {
                                    x: event.clientX,
                                    y: event.clientY,
                                    delta: acc.delta + event.deltaY / 100,
                                };
                            },
                            { x: 0, y: 0, delta: 0 }
                        )
                    ) // Addiere die Events auf
                )
                .subscribe((event) => this.onWheel(event));

            // Resize event handling
            const resize$ = fromEvent(window, 'resize');

            resize$
                .pipe(sampleTime(100))
                .subscribe(() => this.onResizeFast(new Event('resize')));

            resize$
                .pipe(debounceTime(300))
                .subscribe(() => this.onResizeSlow(new Event('resize')));
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


    public async onResizeSlow(_: Event) {
        if (!this.canvas) return;
        if(!this.imageData) return;
        if(!this.imageCanvas) return;

        const canvasWidth = window.innerWidth;
        const canvasHeight = window.innerHeight;

        const aspectRatio = canvasWidth / canvasHeight;
        if(canvasWidth >= canvasHeight) {
            this.imageSize = new ImageSize(this.imageBase, Math.floor(this.imageBase / aspectRatio));
        } else {
            this.imageSize = new ImageSize(Math.floor(this.imageBase * aspectRatio), this.imageBase);
        }

        this.imageData = this.getImageData(this.imageSize);

        this.imageCanvas.width = this.imageSize.Width;
        this.imageCanvas.height = this.imageSize.Height;

        await this._mandelbrotService.refreshedMandelbrotSet(
            this.imageSize,
            this.imageData,
            this.maxIterations
        );

        this.onResizeFast(new Event('resize'));
    }


    public async onResizeFast(_: Event) {
        if (!this.canvas) return;

        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
        this.drawAsync();
    }


    private async drawAsync() {
        if (this.imageContext != null) {
            this.imageContext.putImageData(
                this.imageData,
                0,
                0,
                0,
                0,
                this.imageSize.Width,
                this.imageSize.Height
            );
        }

        // KI-Generiert auf die AnfrageS
        // "Wie kann ich das Image in drawAsync so ausgeben, dass das Verhältnis Width und Height nicht verändert wird?"
        const canvasSize = this.getCanvasSize();
        if (this.imageCanvas) {
            const aspectRatioImage = this.imageSize.Width / this.imageSize.Height;
            const aspectRatioCanvas = canvasSize.Width / canvasSize.Height;
            let drawWidth = canvasSize.Width;
            let drawHeight = canvasSize.Height;

            if (aspectRatioCanvas > aspectRatioImage) {
                drawWidth = canvasSize.Height * aspectRatioImage;
            } else {
                drawHeight = canvasSize.Width / aspectRatioImage;
            }

            const offsetX = (canvasSize.Width - drawWidth) / 2;
            const offsetY = (canvasSize.Height - drawHeight) / 2;

            this.context.drawImage(
                this.imageCanvas,
                0,
                0,
                this.imageSize.Width,
                this.imageSize.Height,
                offsetX,
                offsetY,
                drawWidth,
                drawHeight
            );
        }
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
        const width = this.context.canvas.offsetWidth;
        const height = this.context.canvas.offsetHeight;
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
