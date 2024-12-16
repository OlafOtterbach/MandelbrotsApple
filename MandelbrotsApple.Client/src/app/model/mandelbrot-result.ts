import { MandelbrotSize } from "./mandelbrot-size";
import { ErrorType } from "./error-type";

export interface MandelbrotResult {
  ImageData: string,
  MandelbrotSize: MandelbrotSize,
  Errors: ErrorType[],
  HasErrors: boolean
}
