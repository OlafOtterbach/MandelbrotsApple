import { Component, ElementRef, OnInit, output, ViewChild } from '@angular/core';
import { MandelbrotService } from '../services/mandelbrot-service';

@Component({
  selector: 'app-mandelbrot-view',
  standalone: true,
  templateUrl: './mandelbrot-view.component.html',
  styleUrls: ['./mandelbrot-view.component.css']
})
export class MandelbrotViewComponent implements OnInit {

  //@ViewChild('ImageId') imageElement: HTMLImageElement | undefined = undefined;
  @ViewChild('ImageId', { static: false }) imageRef: HTMLImageElement | undefined = undefined;

  imageData: string = `data:image/jpeg;base64,` + 'AADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADaAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAADsAAD…D/AAAAAAAA7OwAAMgAAADsAAAAAMgAAAAAAAAAAAAAAAAAAAAAAAAAAADI/wAA2gAAyAAAANoAANoAANoAyAAAAAAAAOwA//8A7AAAAAAAAADIyAAAAMgA7OwAAAAA//8AAAAAyMgA7OwA//8A/wAA/wAA7AAA7AAA7AAA7AAA2gAA2gAA2gAA2gAA2gAA2gAAyAAAyAAAyAAAyAAAAMgAAMgAAMgAAMgAAMgAANoAANoAANoAANoAANoAANoAANoAAMgAAMgAyAAAyAAA7AAAAAAAAAAAAAAA7OwAAAD/AADs2gAA/wAAAAAAAAAA2gAAANoAAMgA7AAA2gAAAAD/AADsANoAAAAAAAAAAAAAAAAAAAAAAAAAAADIyMgAAMgAAAD/AADsAMgAAAAA//8A2toAAAAAAAAAAAAAyAAAAAAA2toAAAAAAMgAAAAA/wAAAAAAAAAAAAAAAAAAAADa2toAAAAA7OwAAAAAAAAAAAAAAMgA';

  constructor(private _mandelbrotService: MandelbrotService) {

  }

  async ngOnInit() {
  }

  async ngAfterViewInit() {
    if(this.imageRef === undefined)
      return;

    const imageData = await this._mandelbrotService.getImage();
    const source = `data:image/jpeg;base64,${imageData}`;
    this.imageRef.src = source;
    this.imageData = source;
  }


}
