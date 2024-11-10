import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { MandelbrotService } from '../services/mandelbrot-service';

@Component({
    selector: 'app-mandelbrot-view',
    standalone: true,
    templateUrl: './mandelbrot-view.component.html',
    styleUrls: ['./mandelbrot-view.component.css'],
})
export class MandelbrotViewComponent implements AfterViewInit {
    //@ViewChild('canvasId', { static: false }) canvasRef: ElementRef | undefined = undefined;
    @ViewChild('canvasId', { static: true }) canvasRef!: ElementRef;

    private context!: CanvasRenderingContext2D;
    private imageData!: ImageData;

    constructor(private _mandelbrotService: MandelbrotService) {}

    ngAfterViewInit() {
        const canvas = this.canvasRef.nativeElement as HTMLCanvasElement;
        if (canvas && canvas.getContext) {
            this.context = canvas.getContext('2d')!;
            this.drawAsync();
        }
    }

    private async drawAsync() {
        const width = this.context.canvas.width;
        const height = this.context.canvas.height;
        this.imageData = this.context.getImageData(0, 0, width, height);
        await this._mandelbrotService.getGraphics(
            this.imageData,
            width,
            height,
            0.763,
            0.768,
            0.0999,
            0.103,
            255
        );
        this.context.putImageData(this.imageData, 0, 0, 0, 0, width, height);
    }

    public onResize(_: Event) {
        this.drawAsync();
    }
}
