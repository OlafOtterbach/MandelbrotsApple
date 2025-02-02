import { MandelbrotSize } from "./mandelbrot-size";
import { ErrorType } from "./error-type";

export interface MandelbrotResult {
  ImageData: string,
  BytesPerPixel: number,
  MaxIterations: number,
  MandelbrotSize: MandelbrotSize,
  Errors: ErrorType[],
  HasErrors: boolean
}
