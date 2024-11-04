import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MandelbrotViewComponent } from './mandelbrot-view/mandelbrot-view.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MandelbrotViewComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'MandelbrotsApple.Client';
}
