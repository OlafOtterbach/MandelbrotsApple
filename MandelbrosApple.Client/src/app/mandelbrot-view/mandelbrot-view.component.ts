import { Component, OnInit } from '@angular/core';
import { MandelbrotService } from '../services/mandelbrot-service';

@Component({
  selector: 'app-mandelbrot-view',
  standalone: true,
  templateUrl: './mandelbrot-view.component.html',
  styleUrls: ['./mandelbrot-view.component.css']
})
export class MandelbrotViewComponent implements OnInit {

  constructor(private _mandelbrotService: MandelbrotService) {

  }

  ngOnInit() {
    this._mandelbrotService.getGraphics(800, 600, -2.0, 2-0, -2.0, 2.0, 255);
  }

}
