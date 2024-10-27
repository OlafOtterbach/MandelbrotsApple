import { Injectable } from '@angular/core';
import { MandelbrotWebApiService } from './mandelbrot-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class MandelbrotService {

  constructor(private mandebrotWebApi: MandelbrotWebApiService) { }

  public async getGraphics(
    width: number,
    height: number,
    xMin: number,
    xMax: number,
    yMin: number,
    yMax: number,
    maxIterations: number) {
      const result = await this.mandebrotWebApi.getMandelbrotResult(width, height, xMin, xMax, yMin, yMax, maxIterations);

  }

  public async getImage() : Promise<string> {
    const imageData = await this.mandebrotWebApi.getMandelbrotImage();
    return imageData;
  }

}


/**
    this.imageData = this.context.getImageData(0, 0, width, height);

    let map = imageData.data; //new Uint8ClampedArray(width * 4 * height);


    var index = (row * width * 4) + (col * 4);
    [map[index], map[index + 1], map[index + 2], map[index + 3]] = [red, green, blue, 255];

 */
