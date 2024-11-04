import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { MandelbrotService } from '../services/mandelbrot-service';

@Component({
  selector: 'app-mandelbrot-view',
  standalone: true,
  templateUrl: './mandelbrot-view.component.html',
  styleUrls: ['./mandelbrot-view.component.css']
})
export class MandelbrotViewComponent implements AfterViewInit {

  //@ViewChild('canvasId', { static: false }) canvasRef: ElementRef | undefined = undefined;
  @ViewChild('canvasId') canvasRef!: ElementRef;


  constructor(private _mandelbrotService: MandelbrotService) {
  }

  ngAfterViewInit() {
    const canvas = this.canvasRef.nativeElement;
    const context = canvas.getContext('2d');
    const width = context.canvas.width;
    const height = context.canvas.height;
    let imageData = context.getImageData(0, 0, width, height);
    this._mandelbrotService.getGraphics(imageData, width, height,  0.763, 0.768, 0.0999, 0.103, 255);

    context.putImageData(imageData, 0, 0);
  }

}
