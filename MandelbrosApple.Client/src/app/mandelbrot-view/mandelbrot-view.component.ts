import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MandelbrotService } from '../services/mandelbrot-service';

@Component({
  selector: 'app-mandelbrot-view',
  standalone: true,
  templateUrl: './mandelbrot-view.component.html',
  styleUrls: ['./mandelbrot-view.component.css']
})
export class MandelbrotViewComponent implements OnInit {

  @ViewChild('canvasId', { static: false }) canvasRef: ElementRef | undefined = undefined;


  constructor(private _mandelbrotService: MandelbrotService) {

  }

  async ngOnInit() {
  }

  async ngAfterViewInit() {
    if(this.canvasRef === undefined)
      return;

    const context = this.canvasRef.nativeElement.getContext('2d');

    if(context === undefined)
      return;

    const width = context.canvas.width;
    const height = context.canvas.height;

    // let imageData = context.getImageData(0, 0, width, height);

    // if(imageData === undefined)
    //   return;

    // this._mandelbrotService.getGraphics(imageData, width, height,  0.763, 0.768, 0.0999, 0.103, 255);

    //context.putImageData(imageData, 0, 0);
  }

}
