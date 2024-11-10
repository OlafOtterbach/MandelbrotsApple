import { Injectable } from '@angular/core';
import { MandelbrotWebApiService } from './mandelbrot-web-api.service';

@Injectable({
  providedIn: 'root'
})
export class MandelbrotService {

  constructor(private mandebrotWebApi: MandelbrotWebApiService) { }

  public async getGraphics(
    imageData: ImageData,
    width: number,
    height: number,
    xMin: number,
    xMax: number,
    yMin: number,
    yMax: number,
    maxIterations: number) {
      const result = await this.mandebrotWebApi.getMandelbrotResult(width, height, xMin, xMax, yMin, yMax, maxIterations);
      const map = imageData.data;

      if(!result.hasErrors) {
        let index = 0;
        const codedData = result.imageData;
        for(let i = 0; i < codedData.length; i+=6) {
          const hightRed = codedData[i];
          const lowRed = codedData[i + 1];
          const red = this.convertToByte(hightRed, lowRed);
          const hightGreen = codedData[i + 2];
          const lowGreen = codedData[i + 3];
          const green = this.convertToByte(hightGreen, lowGreen);
          const hightBlue = codedData[i + 4];
          const lowBlue = codedData[i + 5];
          const blue = this.convertToByte(hightBlue, lowBlue);
          [map[index], map[index + 1], map[index + 2], map[index + 3]] = [red, green, blue, 255];
          index += 4;
        }
      }
  }

  private convertToByte(hight: string, low: string) : number {
    const value = this.convertToNibble(hight) << 4 | this.convertToNibble(low);
    return value;
  }

  private convertToNibble(c: string): number {
    switch(c) {
      case "0": return 0;
      case "1": return 1;
      case "2": return 2;
      case "3": return 3;
      case "4": return 4;
      case "5": return 5;
      case "6": return 6;
      case "7": return 7;
      case "8": return 8;
      case "9": return 9;
      case "A": return 10;
      case "B": return 11;
      case "C": return 12;
      case "D": return 13;
      case "E": return 14;
      case "F": return 15;
      default: return 0;
    }
  }
}


/**
    this.imageData = this.context.getImageData(0, 0, width, height);

    let map = imageData.data; //new Uint8ClampedArray(width * 4 * height);


    var index = (row * width * 4) + (col * 4);
    [map[index], map[index + 1], map[index + 2], map[index + 3]] = [red, green, blue, 255];

 */
